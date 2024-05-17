using System.Security;

namespace VprModLib.Serialization
{
    public static class VprFileUtility
    {
        public static bool TryRead(string filepath, out byte[]? fileBytes, out string message)
        {
            fileBytes = null;
            message = $"Unhandled exception in TryRead().";

            try
            {
                // Read the bytes of the file at the specified path.
                fileBytes = File.ReadAllBytes(filepath);
                message = "Success.";
                return true;
            }
            catch (ArgumentNullException)
            {
                message = "The file path is NULL. " + FileIO.INTERNAL_ERROR;
            }
            catch (ArgumentException)
            {
                message = "The file path is empty or contains invalid characters. Please specify a valid path to a Vocaloid project file.";
            }
            catch (PathTooLongException)
            {
                message = "The file path is too long. Adjust your system's maximum file path length settings or move the file to a location with a shorter file path.";
            }
            catch (DirectoryNotFoundException)
            {
                message = "Unable to locate the specified directory. Please make sure the file path points to an existing location.";
            }
            catch (UnauthorizedAccessException)
            {
                message = "Unable to access the specified file. Verify that the file path points to a file (and not a directory), and that you have the necessary permissions to read the file.";
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the specified file. Please make sure the file path points to an existing location.";
            }
            catch (NotSupportedException)
            {
                message = "The file path is in an invalid format. " + FileIO.INTERNAL_ERROR;
            }
            catch (IOException)
            {
                message = "An I/O exception occurred while reading the file. " + FileIO.FATAL_ERROR;
            }
            catch (SecurityException)
            {
                message = "Missing the required permissions to read the file.";
            }
            catch (Exception)
            {
                message = "An unhandled exception occurred while opening the file. " + FileIO.INTERNAL_ERROR;
            }

            return false;
        }
        public static bool TryWrite(string filepath, MemoryStream zipArchiveStream, out string message)
        {
            message = $"Unhandled exception in TryWrite().";
            try
            {
                using (var fileStream = File.Open(filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    zipArchiveStream.CopyTo(fileStream);
                    zipArchiveStream.Dispose();
                }
                
                message = "Success.";
                return true;
            }
            catch (ArgumentNullException)
            {
                message = "The file path is NULL. " + FileIO.INTERNAL_ERROR;
            }
            catch (ArgumentException)
            {
                message = "The file path is empty or contains invalid characters. Please specify a valid path for the new Vocaloid project file.";
            }
            catch (PathTooLongException)
            {
                message = "The file path is too long. Adjust your system's maximum file path length settings or select a location with a shorter path.";
            }
            catch (DirectoryNotFoundException)
            {
                message = "The specified directory is invalid. Please make sure the file path points to a valid location.";
            }
            catch (UnauthorizedAccessException)
            {
                message = "The file path to be overwritten is read-only, hidden, points to a directory, or the required permissions are missing.";
            }
            catch (NotSupportedException)
            {
                message = "The file path is in an invalid format. " + FileIO.INTERNAL_ERROR;
            }
            catch (IOException)
            {
                message = "An I/O exception occurred while writing the file. " + FileIO.FATAL_ERROR;
            }
            catch (SecurityException)
            {
                message = "Missing the required permissions to write the file.";
            }
            catch (Exception)
            {
                message = "An unhandled exception occurred while opening the file. " + FileIO.INTERNAL_ERROR;
            }

            return false;
        }
    }
}