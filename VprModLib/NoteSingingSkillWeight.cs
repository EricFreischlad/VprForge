using System.Text.Json.Serialization;

namespace VprModLib
{
    public class NoteSingingSkillWeight
    {
        /// <summary>
        /// The intensity of the effect of Singing Skill on the "attack" portion of the note. Values ranges from 0 to 127 with 0 being no expression (a flat line) and 127 being the most extreme expression. See <see cref="NoteSingingSkill"/> for more info.
        /// </summary>
        public int Pre { get; set; }
        /// <summary>
        /// The intensity of the effect of Singing Skill on the "release" portion of the note. Values ranges from 0 to 127 with 0 being no expression (a flat line) and 127 being the most extreme expression. See <see cref="NoteSingingSkill"/> for more info.
        /// </summary>
        public int Post { get; set; }
    }
}

namespace VprModLib.Serialization
{
    public class SerializedNoteSingingSkillWeight : ISerialized<NoteSingingSkillWeight>
    {
        public int pre;
        public int post;

        [JsonConstructor]
        public SerializedNoteSingingSkillWeight(int pre, int post)
        {
            this.pre = pre;
            this.post = post;
        }
        public SerializedNoteSingingSkillWeight(NoteSingingSkillWeight model)
        {
            pre = model.Pre;
            post = model.Post;
        }

        public bool IsValid()
        {
            return pre >= 0 && pre <= 127
                && post >= 0 && post <= 127;
        }

        public NoteSingingSkillWeight ToModel()
        {
            return new NoteSingingSkillWeight
            {
                Pre = pre,
                Post = post,
            };
        }
    }
}