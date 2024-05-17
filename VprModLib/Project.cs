namespace VprModLib
{
    public class Project
    {
        public Sequence Sequence { get; }
        public AudioFiles Audio { get; }

        public Project(Sequence sequence, AudioFiles audio)
        {
            Sequence = sequence;
            Audio = audio;
        }
    }
}