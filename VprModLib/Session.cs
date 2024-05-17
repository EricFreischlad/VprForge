namespace VprModLib
{
    public class Session
    {
        /// <summary>
        /// The unmodified, original bytes of the Vocaloid project file. Preserves any files/comments made by other modders. If <see langword="null"/>, this project was created artificially during this session.
        /// </summary>
        public byte[]? OriginalFileBytes { get; }

        /// <summary>
        /// The VPR Forge model of the project file.
        /// </summary>
        public Project Project { get; }

        public Session(Project project, byte[]? originalFileBytes)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            OriginalFileBytes = originalFileBytes;
        }
    }
}