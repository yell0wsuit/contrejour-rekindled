using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;
using Mokus2D.Visual.Data;

namespace Default.Namespace
{
    public class ParticleSystem(TextureData config) : Sprite(config)
    {
        public List<Particle> Particles => particles;

        public bool Paused
        {
            get => paused; set => paused = value;
        }

        public ParticleSystem(string textureName)
            : this(ClipFactory.GetAnchorConfig(textureName))
        {
        }

        public ParticleSystem(string textureName, int count)
            : this(ClipFactory.GetAnchorConfig(textureName), count)
        {
        }

        public ParticleSystem(TextureData config, int count)
            : this(config)
        {
            AddParticles(count);
        }

        public void AddParticles(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _ = AddParticle(new Vector2(0f, 0f));
            }
        }

        public void SetParticleVisible(Particle particle)
        {
            if (!particle.Visible)
            {
                particles.Add(particle);
                if (invisibleParticles.Count > 0)
                {
                    invisibleParticles.RemoveAt(invisibleParticles.Count - 1);
                }
                particle.Visible = true;
                OnShowParticle(particle);
            }
        }

        protected virtual void OnShowParticle(Particle particle)
        {
        }

        public Particle AddOrGetInvisible()
        {
            if (invisibleParticles.Count > 0)
            {
                Particle particle = invisibleParticles[invisibleParticles.Count - 1];
                SetParticleVisible(particle);
                return particle;
            }
            return AddParticle();
        }

        public Particle AddParticleWithFrame(int frame)
        {
            if (frame >= config.Frames)
            {
                throw new ArgumentOutOfRangeException("frame");
            }
            Particle particle = CreateParticle();
            particles.Add(particle);
            if (Config.Frames > 1)
            {
                SetFrameFrame(particle, frame);
            }
            else
            {
                particle.AnchorInPixels = Config.Anchor * Config.Size;
                particle.Frame = 0;
            }
            return particle;
        }

        public void SetFrameFrame(Particle particle, int frame)
        {
            Vector2 vector = Config.Anchor * Config.Size;
            particle.Frame = frame;
            particle.AnchorInPixels = vector;
        }

        public void ShowAllParticles()
        {
            while (invisibleParticles.Count > 0)
            {
                SetParticleVisible(invisibleParticles[invisibleParticles.Count - 1]);
            }
        }

        public void HideAllParticles()
        {
            foreach (Particle particle in particles)
            {
                particle.Visible = false;
            }
        }

        public Particle AddParticle()
        {
            return AddParticleWithFrame(Maths.Random(Config.Frames));
        }

        public Particle AddParticle(Vector2 position)
        {
            Particle particle = AddParticle();
            particle.Position = position;
            return particle;
        }

        public virtual Particle CreateParticle()
        {
            return new Particle(this);
        }

        public override void Update(float time)
        {
            if (paused)
            {
                return;
            }
            base.Update(time);
            cachedInvisible.Clear();
            foreach (Particle particle in particles)
            {
                UpdateParticleTime(particle, time);
                if (!particle.Visible)
                {
                    cachedInvisible.Add(particle);
                }
            }
            invisibleParticles.AddRange(cachedInvisible);
            for (int i = 0; i < cachedInvisible.Count; i++)
            {
                _ = particles.Remove(cachedInvisible[i]);
            }
        }

        public virtual void UpdateParticleTime(Particle particle, float time)
        {
            if (particle.UpdateEnabled)
            {
                particle.Update(time);
            }
        }

        protected override void DrawSprite(VisualState state, Color color)
        {
            foreach (Particle particle in particles)
            {
                if (particle.Visible)
                {
                    Color color2 = color * particle.OpacityFloat;
                    Vector2 vector = CocosUtil.toRetina(particle.Position);
                    batch.Draw(Texture, vector, new global::Microsoft.Xna.Framework.Rectangle?(Config.FramesBounds[particle.Frame]), color2, Maths.ToRadians(particle.Rotation), particle.AnchorInPixels, particle.ScaleVec * state.SpritesScaleFactor, SpriteEffects.None, 0f);
                }
            }
        }

        protected List<Particle> particles = new();

        protected List<Particle> invisibleParticles = new();

        private List<Particle> cachedInvisible = new();

        protected bool paused;
    }
}
