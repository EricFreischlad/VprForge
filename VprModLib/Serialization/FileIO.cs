namespace VprModLib.Serialization
{
    public static class FileIO
    {
        public const string FATAL_ERROR = "If the problem persists, it may not be something VprModLib can solve.";
        public const string FILE_CORRUPT = "It may not be a Vocaloid project, or the project may be corrupted.";
        public const string INTERNAL_ERROR = "This is an internal error within VprModLib.";

        public static bool TryRead(string filepath, out Session? session, out string message)
        {
            session = null;

            try
            {
                if (!VprFileUtility.TryRead(filepath, out var originalFileBytes, out message))
                {
                    return false;
                }

                if (!VprZipUtility.TryUnzip(originalFileBytes!, out string? sequenceJsonStr, out var audioFiles, out message))
                {
                    return false;
                }

                if (!VprJsonUtility.TryDeserialize(sequenceJsonStr!, out var serializedSequence, out message))
                {
                    return false;
                }

                if (!VprModelUtility.TryBuildModel(serializedSequence!, out var sequence, out message))
                {
                    return false;
                }

                var project = new Project(sequence!, audioFiles!);

                session = new Session(project, originalFileBytes);

                // Success message should be set already.
                return true;
            }
            catch (Exception e)
            {
                message = "An unhandled exception ocurred when trying to open the file: " + e;
            }
            
            return false;
        }
        public static bool TryReadDeconstructed(string filepath, out SerializedSequence? sequence, out AudioFiles? audioFiles, out byte[]? originalFileBytes, out string message)
        {
            sequence = null;
            audioFiles = null;
            originalFileBytes = null;

            try
            {
                if (!VprFileUtility.TryRead(filepath, out originalFileBytes, out message))
                {
                    return false;
                }

                if (!VprZipUtility.TryUnzip(originalFileBytes!, out string? sequenceJsonStr, out audioFiles, out message))
                {
                    return false;
                }

                if (!VprJsonUtility.TryDeserialize(sequenceJsonStr!, out sequence, out message))
                {
                    return false;
                }

                // Success message should be set already.
                return true;
            }
            catch (Exception e)
            {
                message = "An unhandled exception ocurred when trying to open the file: " + e;
            }

            return false;
        }
        public static bool TryWrite(string filepath, Session session, out string message)
        {
            try
            {
                if (!VprModelUtility.TryBuildSerialized(session.Project.Sequence, out var serializedSequence, out message))
                {
                    return false;
                }

                if (!VprJsonUtility.TrySerialize(serializedSequence!, out var sequenceJsonStr, out message))
                {
                    return false;
                }

                if (!VprZipUtility.TryZip(sequenceJsonStr!, session.Project.Audio, session.OriginalFileBytes, out var zipArchiveStream, out message))
                {
                    return false;
                }

                if (!VprFileUtility.TryWrite(filepath, zipArchiveStream!, out message))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                message = "An unhandled exception ocurred when trying to write the file: " + e;
            }

            return true;
        }
        public static bool TryWriteFromSerializedSequence(string filepath, SerializedSequence sequence, AudioFiles audioFiles, byte[]? originalFileBytes, out string message)
        {
            try
            {
                if (!VprJsonUtility.TrySerialize(sequence, out var sequenceJsonStr, out message))
                {
                    return false;
                }

                if (!VprZipUtility.TryZip(sequenceJsonStr!, audioFiles, originalFileBytes, out var zipArchiveStream, out message))
                {
                    return false;
                }

                if (!VprFileUtility.TryWrite(filepath, zipArchiveStream!, out message))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                message = "An unhandled exception ocurred when trying to write the file: " + e;
            }

            return true;
        }
    }
}