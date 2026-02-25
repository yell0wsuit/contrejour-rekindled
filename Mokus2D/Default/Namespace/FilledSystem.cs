using System;

using Microsoft.Xna.Framework;

using Mokus2D.Visual.Data;

namespace Mokus2D.Default.Namespace
{
    public class FilledSystem(TextureData config) : ParticleSystem(config)
    {
        public FilledSystem(string name)
            : this(ClipFactory.GetAnchorConfig(name))
        {
        }

        public void Fill(CGSize size)
        {
            int num = (int)Math.Ceiling((double)(size.Width / (Config.Size.Width / 1f)));
            int num2 = (int)Math.Ceiling((double)(size.Height / (Config.Size.Height / 1f)));
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num2; j++)
                {
                    _ = AddParticle(new Vector2(i * Config.Size.Width / 1f, j * Config.Size.Height / 1f));
                }
            }
        }
    }
}
