using System.Text.Json.Serialization;

namespace VprModLib
{
    public class MasterTrack
    {
        public int SamplingRate { get; set; }
        public Loop Loop { get; set; } = new Loop();
        public TempoTimeline Tempo { get; set; } = new TempoTimeline();
        public TimeSignatureTimeline TimeSig { get; set; } = new TimeSignatureTimeline();
        public VolumeTimeline Volume { get; set; } = new VolumeTimeline();
        
        // Omitted if there are no audio effects at the track level.
        public List<Effect> AudioEffects { get; } = new List<Effect>();
    }
}
namespace VprModLib.Serialization
{
    public class SerializedMasterTrack : ISerialized<MasterTrack>
    {
        public int samplingRate;
        public SerializedLoop loop;
        public SerializedTempoTimeline tempo;
        public SerializedTimeSignatureTimeline timeSig;
        public SerializedVolumeTimeline volume;
        public SerializedEffect[]? audioEffects;

        [JsonConstructor]
        public SerializedMasterTrack(int samplingRate, SerializedLoop loop, SerializedTempoTimeline tempo, SerializedTimeSignatureTimeline timeSig, SerializedVolumeTimeline volume, SerializedEffect[]? audioEffects)
        {
            this.samplingRate = samplingRate;
            this.loop = loop;
            this.tempo = tempo;
            this.timeSig = timeSig;
            this.volume = volume;
            this.audioEffects = audioEffects;
        }
        public SerializedMasterTrack(MasterTrack model)
        {
            samplingRate = model.SamplingRate;
            loop = new SerializedLoop(model.Loop);
            tempo = new SerializedTempoTimeline(model.Tempo);
            timeSig = new SerializedTimeSignatureTimeline(model.TimeSig);
            volume = new SerializedVolumeTimeline(model.Volume);
            audioEffects = model.AudioEffects.Count != 0 ?
                model.AudioEffects.Select(ae => new SerializedEffect(ae)).ToArray() :
                null;
        }

        public bool IsValid()
        {
            // TODO: Sampling rate struct.
            return (samplingRate == 44100
                || samplingRate == 48000
                || samplingRate == 96000
                || samplingRate == 192000)
                && loop is { }
                && loop.IsValid()
                && tempo is { }
                && tempo.IsValid()
                && timeSig is { }
                && timeSig.IsValid()
                && volume is { }
                && volume.IsValid()
                && (audioEffects is null
                    || audioEffects.All(ae => ae.IsValid()));

        }

        public MasterTrack ToModel()
        {
            var model = new MasterTrack()
            {
                SamplingRate = samplingRate,
                Loop = loop.ToModel(),
                Tempo = tempo.ToModel(),
                TimeSig = timeSig.ToModel(),
                Volume = volume.ToModel(),
            };
            if (audioEffects is { })
            {
                model.AudioEffects.AddRange(audioEffects.Select(ae => ae.ToModel()));
            }
            return model;
        }
    }
}