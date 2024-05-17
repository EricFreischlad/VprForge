using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VprModLib.AudioEffects
{
    public partial class EffectType
    {
        static EffectType()
        {
            _registeredEffectTypes = new List<EffectType>()
            {
                /*
                 *      Audio Effects.
                 */
                
                // Auto Pan.
                new EffectType("CDB4C488-BB24-45cb-8277-A245CB228BA8", "Auto Pan", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Depth", 0.6299212574958801, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("LFO Freq", 0.009009009227156639, (0.0, 1.0)),
                    new EnumDoubleEffectParameterDefinition("Shape Type", "Sine", new List<string>()
                    {
                        "Sine",
                        "Square",
                    }),
                    new SyncNoteEffectParameterDefinition("Sync Note"),
                    new TempoSyncEffectParameterDefinition("Tempo Sync"),
                }),

                // Change Pitch (Audio parts only).
                new EffectType("C5557794-2E4A-4F82-8DE9-B66A32A90D29", "Change Pitch", new List<EffectParameterDefinition>()
                {
                    new KnobIntEffectParameterDefinition("Pitch Shift", 0, (-12, 12)),
                }),

                // Chorus.
                new EffectType("F36904B7-5032-4cbe-AA48-C95440732F9D", "Chorus", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Delay Offset", 0.19191919267177583, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Depth", 0.25196850299835207, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Dry/Wet", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Feedback", 0.5396825671195984, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("LFO Freq", 0.018036073073744775, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Spatial", 0.75, (0.0, 1.0)),
                    new SyncNoteEffectParameterDefinition("Sync Note"),
                    new TempoSyncEffectParameterDefinition("Tempo Sync"),
                    new EnumDoubleEffectParameterDefinition("Type", "Chorus 1", new List<string>()
                    {
                        "Chorus 1",
                        "Chorus 2",
                        "Flanger",
                    }),
                }),

                // Compressor.
                new EffectType("4D1F311D-DA90-459d-B581-003D003A1E5E", "Compressor", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Attack", 0.5619469285011292, (0.0, 1.0)),
                    new EnumDoubleEffectParameterDefinition("Knee", "Medium", new List<string>()
                    {
                        "Soft",
                        "Medium",
                        "Hard",
                    }),
                    new KnobDoubleEffectParameterDefinition("Ratio", 0.25, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Release", 0.489130437374115, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Threshold", 0.37037035822868349, (0.0, 1.0)),
                }),

                // De-esser.
                new EffectType("BCE39D97-5E31-4bea-8FAB-C37BDD0FBAA4", "De-esser", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Detect Freq", 0.7666666507720947, (0.0, 1.0)),
                    new ButtonDoubleEffectParameterDefinition("Monitor", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Threshold", 0.5625, (0.0, 1.0)),
                }),

                // Delay.
                new EffectType("5AEA0D66-9D3E-4a53-B46B-925C51B94499", "Delay", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Dry/Wet", 0.3095238208770752, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("High Damp", 1.0, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Lch Delay1", 0.028797097504138948, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Lch FB Gain", 0.6269841194152832, (0.0, 1.0)),
                    new SyncNoteEffectParameterDefinition("Lch Sync Note"),
                    new ButtonDoubleEffectParameterDefinition("Mode", 0.0, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Rch Delay1", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Rch FB Gain", 0.5, (0.0, 1.0)),
                    new SyncNoteEffectParameterDefinition("Rch Sync Note"),
                    new KnobDoubleEffectParameterDefinition("Spatial", 0.5, (0.0, 1.0)),
                    new TempoSyncEffectParameterDefinition("Tempo Sync"),
                }),

                // Distortion.
                new EffectType("9F301F02-A705-4b7b-9D51-E778E8338374", "Distortion", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Drive", 0.0, (0.0, 1.0)),
                    new EnumDoubleEffectParameterDefinition("Lo-Fi Type", "Off", new List<string>()
                    {
                        "Off",
                        "1",
                        "2",
                        "3",
                    }),
                    new KnobDoubleEffectParameterDefinition("Lo-Fi Amount", 0.0, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Output", 0.5, (0.0, 1.0)),
                }),

                // Equalizer.
                new EffectType("C7E4ED13-FF1E-4fc2-a87c-65604789469D", "Equalizer", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("High F", 0.8125, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("High G", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("High Mid F", 0.6666666865348816, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("High Mid G", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("High Mid Q", 0.20000000298023225, (0.0, 1.0)),
                    new ButtonDoubleEffectParameterDefinition("HPF", 1.0, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Low F", 0.4117647111415863, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Low G", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Low Mid F", 0.5333333611488342, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Low Mid G", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Low Mid Q", 0.20000000298023225, (0.0, 1.0)),
                }),

                // Gain.
                new EffectType("C7CE3316-6FFD-4140-8a7c-0AA8B8108549", "Gain", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Gain", 0.7142857313156128, (0.0, 1.0)),
                }),

                // Phaser.
                new EffectType("30F75AB7-3B14-4439-A4B8-932A63A5F31E", "Phaser", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Depth", 0.874015748500824, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Dry/Wet", 0.5, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Feedback Level", 0.817460298538208, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Phase Offset", 0.5826771855354309, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Rate", 0.019999999552965165, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Stage", 0.4285714328289032, (0.0, 1.0)),
                    new TempoSyncEffectParameterDefinition("Tempo Sync"),
                    new SyncNoteEffectParameterDefinition("Sync Note"),
                    new EnumDoubleEffectParameterDefinition("Type", "Phaser 1", new List<string>()
                    {
                        "Phaser 1",
                        "Phaser 2",
                    }),
                }),

                // Reverb.
                new EffectType("751EF2C0-4229-4ea7-AA0F-82EB5821D20E", "Reverb", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("Initial Delay", 0.015748031437397004, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Mix", 1.0, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("Reverb Time", 0.3333333432674408, (0.0, 1.0)),
                    new EnumDoubleEffectParameterDefinition("Type", "Hall", new List<string>()
                    {
                        "Hall",
                        "Room",
                        "Plate",
                    }),
                }),

                // Reverse (Audio parts only).
                new EffectType("A05491C0-D042-454A-A5E3-7D83B5234525", "Reverse", new List<EffectParameterDefinition>()
                {
                    new ButtonIntEffectParameterDefinition("Reverse Wave", 0, (0, 1)),
                }),

                // Tremolo.
                new EffectType("3213F12A-421C-43a8-B844-7FE8C28A0D64", "Tremolo", new List<EffectParameterDefinition>()
                {
                    new KnobDoubleEffectParameterDefinition("AM Depth", 0.4409448802471161, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("LFO Freq", 0.0, (0.0, 1.0)),
                    new KnobDoubleEffectParameterDefinition("PM Depth", 0.0, (0.0, 1.0)),
                    new EnumDoubleEffectParameterDefinition("Shape Type", "Sine", new List<string>()
                    {
                        "Sine",
                        "Square",
                    }),
                    new SyncNoteEffectParameterDefinition("Sync Note"),
                    new TempoSyncEffectParameterDefinition("Tempo Sync"),
                }),

                /*
                 *      MIDI Effects.
                 */

                // Breath.
                new EffectType("Breath", "Breath", new List<EffectParameterDefinition>()
                {
                    new KnobIntEffectParameterDefinition("Exhalation", 5, (0, 10)),
                    new EnumIntEffectParameterDefinition("Mode", "Sometimes", new List<string>()
                    {
                        "Often",
                        "Sometimes",
                        "Rarely",
                    }),
                    new EnumIntEffectParameterDefinition("Type", "Female", new List<string>()
                    {
                        "Female",
                        "Male",
                    }),
                }),

                // Default Lyric.
                new EffectType("DefaultLyric", "Default Lyric", new List<EffectParameterDefinition>()
                {
                    new StringEffectParameterDefinition("CHS", "a"),
                    new StringEffectParameterDefinition("ENG", "Ooh"),
                    new StringEffectParameterDefinition("ESP", "a"),
                    new StringEffectParameterDefinition("JPN", "あ"),
                    new StringEffectParameterDefinition("KOR", "아"),
                }),

                // Robot Voice.
                new EffectType("RobotVoice", "Robot Voice", new List<EffectParameterDefinition>()
                {
                    new EnumIntEffectParameterDefinition("Mode", "Normal", new List<string>()
                    {
                        "Hard",
                        "Normal",
                        "Soft",
                    }),
                }),

                // Singing Skill.
                new EffectType("SingingSkill", "Singing Skill", new List<EffectParameterDefinition>()
                {
                    new KnobIntEffectParameterDefinition("Amount", 3, (0, 10)),
                    new StringEffectParameterDefinition("Name", "3710C91F-879C-4002-9244-1B48081C1C08"),    // Default is "Clean".
                    new KnobIntEffectParameterDefinition("Skill", 3, (0, 10)),
                }),
                
                // Voice Color.
                new EffectType("VoiceColor", "Voice Color", new List<EffectParameterDefinition>()
                {
                    new KnobIntEffectParameterDefinition("Air", 0, (0, 127)),
                    new KnobIntEffectParameterDefinition("Breathiness", 0, (0, 127)),
                    new KnobIntEffectParameterDefinition("Character", 0, (-64, 63)),
                    new KnobIntEffectParameterDefinition("Exciter", 0, (-64, 63)),
                    new KnobIntEffectParameterDefinition("Growl", 0, (0, 127)),
                    new KnobIntEffectParameterDefinition("Mouth", 0, (-127, 0)),
                }),

            }.ToDictionary(et => et.ID);
        }
    }
}
