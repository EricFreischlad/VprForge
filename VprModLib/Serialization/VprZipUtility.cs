using System.Diagnostics;
using System.IO.Compression;

namespace VprModLib.Serialization
{
    public static class VprZipUtility
    {
        // IMPORANT: If these paths are changed or made into variables, TryUnzip and TryZip MUST be modified to sanitize their inputs.
        public const string SEQUENCE_PATH = "Project\\sequence.json";
        public const string AUDIO_DIR_PATH = "Project\\Audio";

        public static string[] UNSAFE_SUBSTRINGS = new string[]
        {
            "../",
            "..\\",
            "./",
            ".\\",
            ":",
        };

        /// <summary>
        /// Try to extract the internal "sequence.json" string and the audio files from the bytes of a Vocaloid project file. Returns <see langword="true"/> if the process succeeded and emits the sequence file as a JSON string and the audio files as a dictionary (filename including extension : file bytes). Returns <see langword="false"/> if the process failed and emits a message explaining how the process failed.
        /// </summary>
        /// <param name="bytes">File bytes for the ".VPR" Vocaloid project file.</param>
        /// <param name="sequenceJsonStr">The "sequence" JSON file string within the project archive.</param>
        /// <param name="audioFiles">A dictionary of all files found within the "Audio" subdirectory within the project archive. Structured as the raw bytes for the file, keyed by the filename</param>
        /// <param name="message">An error message explaining how the process failed.</param>
        /// <returns><see langword="true"/> if successful, otherwise <see langword="false"/></returns>
        public static bool TryUnzip(byte[] bytes, out string? sequenceJsonStr, out AudioFiles? audioFiles, out string message)
        {
            sequenceJsonStr = string.Empty;
            audioFiles = null;
            message = "Unhandled exception in TryUnzip().";

            if (bytes is null)
            {
                message = "Provided bytes were NULL while creating a stream to UnZIP. " + FileIO.INTERNAL_ERROR;
                return false;
            }

            // Create a memory stream from the file bytes.
            using MemoryStream? byteStream = new MemoryStream(bytes, false);

            ZipArchive zipArchive;
            try
            {
                // Read the file bytes as a zip archive.
                zipArchive = new ZipArchive(byteStream, ZipArchiveMode.Read, true);
            }

            // ArgNullEx handled above.
            // ArgEx not possible.

            catch (InvalidDataException)
            {
                message = "The file is not in the expected format. " + FileIO.FILE_CORRUPT;
                return false;
            }

            var audioFileDict = new Dictionary<string, byte[]>();

            // Iterate all ZIP archive entries...
            foreach (var archiveEntry in zipArchive.Entries)
            {
                var entryPath = archiveEntry.FullName;

                // Skip unsafe paths. Prevents certain path-based attacks.
                if (!IsPathSafe(entryPath))
                {
                    continue;
                }

                // Catch and read "Project/sequence.json". Intentionally case-sensitive for strictness.
                if (entryPath.Equals(SEQUENCE_PATH, StringComparison.Ordinal))
                {
                    try
                    {
                        // Create a stream from the entry.
                        var sequenceReader = new StreamReader(archiveEntry.Open());

                        // Read the file text.
                        sequenceJsonStr = sequenceReader.ReadToEnd();
                    }
                    catch (OutOfMemoryException)
                    {
                        message = "There is not enough memory left to read the internal sequence file.";
                        return false;
                    }
                    catch (IOException)
                    {
                        message = "An I/O exception occurred while reading the internal sequence file. " + FileIO.FATAL_ERROR;
                        return false;
                    }
                }

                // "If the file is in the Audio directory, but not in any directory below it."
                else if (entryPath.StartsWith(AUDIO_DIR_PATH, StringComparison.InvariantCulture))
                {
                    var filepathBelowAudioDir = entryPath[AUDIO_DIR_PATH.Length..];

                    if (!filepathBelowAudioDir.Contains('/', StringComparison.InvariantCulture))
                    {
                        using var audioFileStream = archiveEntry.Open();
                        using var ms = new MemoryStream();
                        audioFileStream.CopyTo(ms);
                        audioFileDict[filepathBelowAudioDir] = ms.ToArray();
                    }
                }

                // Any other file is ignored. Other modders can have fun.
            }

            zipArchive.Dispose();

            // Verify sequence was found.
            if (string.IsNullOrEmpty(sequenceJsonStr))
            {
                message = $"The project file does not contain an internal sequence file. " + FileIO.FILE_CORRUPT;
                return false;
            }

            audioFiles = new AudioFiles(audioFileDict);

            message = "Success.";
            return true;
        }

        /// <summary>
        /// Try to create a new Vocaloid project file from the internal "sequence.json" string and the audio files. Returns <see langword="true"/> if the process succeeded and emits the open memory stream to be written to disk. Returns <see langword="false"/> if the process failed and emits a message explaining how the process failed. Note: If this returns <see langword="true"/>, remember to dispose the memory stream when finished.
        /// </summary>
        /// <param name="sequenceJSON">The "sequence" JSON file string within the project archive.</param>
        /// <param name="audioFiles">The object which manages the raw audio data stored within the project.</param>
        /// <param name="originalFileBytes">File bytes for the ".VPR" Vocaloid project file at the time it was read from the disk. This is used to copy over any non-standard entries in the archive, as well as any comments.</param>
        /// <param name="message">An error message explaining how the process failed.</param>
        /// <returns><see langword="true"/> if successful, otherwise <see langword="false"/></returns>
        public static bool TryZip(string sequenceJSON, AudioFiles audioFiles, byte[]? originalFileBytes, out MemoryStream? zipArchiveStream, out string message)
        {
            message = "Unhandled exception in TryZip().";

            if (string.IsNullOrEmpty(sequenceJSON))
            {
                zipArchiveStream = null;

                message = "The JSON string for the sequence was NULL or empty. " + FileIO.INTERNAL_ERROR;
                return false;
            }

            // If the original file bytes were provided, they will be used to preserve any other files, JSON properties, or comments which existed in the original project as a result of modding.
            ZipArchive? originalZipArchive = null;
            if (originalFileBytes != null)
            {
                Stream? originalFileStream = new MemoryStream(originalFileBytes);

                try
                {
                    originalZipArchive = new ZipArchive(originalFileStream, ZipArchiveMode.Read, true);
                }
                catch (Exception)
                {
                    // Something went wrong opening the original file bytes. Proceed as if creating a project from scratch.
                }
            }

            // Create a new MemoryStream (some bytes we're eventually gonna write to the disk).
            zipArchiveStream = new MemoryStream();

            ZipArchive zipArchive;
            try
            {
                // Create a new ZIP archive from the stream.
                zipArchive = new ZipArchive(zipArchiveStream, ZipArchiveMode.Create, true);
            }
            catch (InvalidDataException)
            {
                // Like, how would we get this exception though?
                // Presumably, TryUnzip was used when opening the file the first time.
                // If silly devs modify the "original file bytes" during the session, then this could trigger.

                message = "The original file is not in the expected format (InvalidDataException in VprZipUtility.TryZip). Most likely a developer error.";
                return false;
            }

            // Sequence:

            // Create an entry for the "sequence.json" file.
            // IMPORTANT: This MUST be written without any compression or Vocaloid can't read it.
            var sequenceEntry = zipArchive.CreateEntry(SEQUENCE_PATH, CompressionLevel.NoCompression);

            // Write the JSON string to the archive entry.
            using (var sequenceStream = sequenceEntry.Open())
            {
                using var sequenceWriter = new StreamWriter(sequenceStream, System.Text.Encoding.UTF8);
                sequenceWriter.Write(sequenceJSON);
            }

            // If this was made from an existing project file,
            if (originalZipArchive is { })
            {
                try
                {
                    // If the original file had any comments on the sequence file, save them.
                    var originalSequenceEntry = originalZipArchive!.GetEntry(SEQUENCE_PATH);
                    if (originalSequenceEntry is { } && !string.IsNullOrEmpty(originalSequenceEntry.Comment))
                    {
                        sequenceEntry.Comment = originalSequenceEntry.Comment;
                    }
                }
                catch (Exception)
                {
                    // The original project file didn't have a sequence or something. Just roll with it.
                }
            }

            // Audio files:
            if (audioFiles is { })
            {
                // For each audio file...
                foreach (var (filename, bytes) in audioFiles.GetForSerialization())
                {
                    var audioFilePath = $"{AUDIO_DIR_PATH}\\{filename}";

                    // Skip unsafe paths. Prevents certain path-based attacks.
                    if (!IsPathSafe(audioFilePath))
                    {
                        continue;
                    }

                    // Create an entry for the audio file.
                    // IMPORTANT: This MUST be written without any compression or Vocaloid can't read it.
                    var audioFileEntry = zipArchive.CreateEntry(audioFilePath, CompressionLevel.NoCompression);

                    // Write the audio file bytes to the archive entry.
                    using (var audioFileStream = audioFileEntry.Open())
                    {
                        var audioFileWriter = new BinaryWriter(audioFileStream);
                        audioFileWriter.Write(bytes);
                    }

                    // If this was made from an existing project file,
                    if (originalZipArchive is { })
                    {
                        try
                        {
                            // If the original file had this audio file and had any comments on it, save them.
                            var originalAudioFileEntry = originalZipArchive!.GetEntry(audioFilePath);
                            if (originalAudioFileEntry is { } && !string.IsNullOrEmpty(originalAudioFileEntry.Comment))
                            {
                                audioFileEntry.Comment = originalAudioFileEntry.Comment;
                            }
                        }
                        catch (Exception)
                        {
                            // The original project file didn't have this audio file. That's fine.
                        }
                    }
                }
            }

            // If this was made from an existing project file,
            if (originalZipArchive is { })
            {
                // Copy every remaining archive entry from the original file to the new file.
                foreach (var origEntry in originalZipArchive.Entries)
                {
                    // Skip unsafe paths. Prevents certain path-based attacks.
                    if (!IsPathSafe(origEntry.FullName))
                    {
                        continue;
                    }

                    // Skip the original sequence!!
                    if (origEntry.FullName == SEQUENCE_PATH)
                    {
                        continue;
                    }

                    var entry = zipArchive.CreateEntry(origEntry.FullName);
                    entry.Comment = origEntry.Comment;
                    entry.ExternalAttributes = origEntry.ExternalAttributes;
                    entry.LastWriteTime = origEntry.LastWriteTime;

                    using var origEntryStream = origEntry.Open();
                    using var newEntryStream = entry.Open();
                    origEntryStream.CopyTo(newEntryStream);
                }

                originalZipArchive.Dispose();
            }

            zipArchive.Dispose();
            zipArchiveStream.Position = 0;

            message = "Success.";
            return true;
        }
        private static bool IsPathSafe(string path)
        {
            // Not a complete defense against directory traversal attacks, but good enough.
            
            if (Path.IsPathRooted(path))
            {
                return false;
            }
            foreach (var substring in UNSAFE_SUBSTRINGS)
            {
                if (path.Contains(substring))
                {
                    return false;
                }
            }
            return true;
        }
    }
}