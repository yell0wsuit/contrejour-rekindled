using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class LianaBodyClip : ContreJourBodyClip
    {
        public LianaBodyClip(LevelBuilderBase _builder, LianaData data, Node _clip, Hashtable _config)
            : base(_builder, data.Bodies[0], _clip, _config)
        {
            Color black_COLOR = ContreJourConstants.BLACK_COLOR;
            if (_config.ContainsKey("alpha"))
            {
                black_COLOR.A = (byte)_config.GetInt("alpha");
            }
            clipContent = new LianaSprite(data, black_COLOR);
            clip = clipContent;
            _builder.Add(clipContent, -3);
            parts = new ArrayList();
            for (int i = 1; i < data.Bodies.Count - 1; i++)
            {
                LianaPart lianaPart = new(data.Bodies[i]);
                parts.Add(lianaPart);
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            foreach (object obj in parts)
            {
                LianaPart lianaPart = (LianaPart)obj;
                lianaPart.Update(time);
            }
            clipContent.Update(time);
        }

        public override void UpdatePosition()
        {
        }

        public override void UpdateRotation()
        {
        }

        protected LianaData data;

        protected ArrayList parts;

        protected LianaSprite clipContent;
    }
}
