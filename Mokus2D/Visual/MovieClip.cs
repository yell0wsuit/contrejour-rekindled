using System;

using Microsoft.Xna.Framework;

using Mokus2D.Util;
using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class MovieClip(TextureData config) : Sprite(config)
    {
        public bool Rewind
        {
            get => rewind; set => rewind = value;
        }

        public bool Stoped
        {
            get => stoped; set => stoped = value;
        }

        public uint TotalFrames => (uint)Config.Frames;

        public float MinFrame
        {
            get => minFrame;
            set
            {
                minFrame = value;
                if (currentFrame < minFrame)
                {
                    CurrentFrame = minFrame;
                }
            }
        }

        public float MaxFrame
        {
            get => maxFrame;
            set
            {
                maxFrame = value - 0.0001f;
                if (currentFrame > maxFrame)
                {
                    CurrentFrame = maxFrame;
                }
            }
        }

        public float CurrentFrame
        {
            get => currentFrame;
            set
            {
                if ((value > maxFrame || value < minFrame) && !stoped && !Repeat)
                {
                    if (value > maxFrame)
                    {
                        value = maxFrame;
                    }
                    if (value < minFrame)
                    {
                        value = minFrame;
                    }
                    stoped = true;
                }
                while (value > maxFrame)
                {
                    value = value - maxFrame + minFrame;
                }
                while (value < minFrame)
                {
                    value = value - minFrame + maxFrame;
                }
                currentFrame = value;
                if ((uint)currentFrame != frameValue)
                {
                    frameValue = (uint)currentFrame;
                    currentFrameBounds = config.FramesBounds[(int)frameValue];
                }
            }
        }

        public event Action EndEvent;

        public override void Update(float time)
        {
            base.Update(time);
            if (!stoped)
            {
                int num = rewind ? (-1) : 1;
                float num2 = time * fps * Speed * num;
                float num3 = currentFrame + num2;
                CurrentFrame = num3;
                if ((num3 > maxFrame && !rewind) || (num3 < minFrame && rewind))
                {
                    EndEvent.Dispatch();
                }
            }
        }

        public void GotoAndPlay(uint frame)
        {
            stoped = false;
            CurrentFrame = frame;
        }

        public void GotoAndStop(uint frame)
        {
            stoped = true;
            CurrentFrame = frame;
        }

        protected override Rectangle? GetTileRectangle()
        {
            return new global::Microsoft.Xna.Framework.Rectangle?(currentFrameBounds);
        }

        private const float EPSILON = 0.0001f;

        public bool Repeat = true;

        public float Speed = 1f;

        private float currentFrame;

        private Rectangle currentFrameBounds = config.Config.FramesBounds[0];

        private readonly float fps = 30f;

        private uint frameValue = 1U;

        private float maxFrame = config.Config.Frames - 0.0001f;

        private float minFrame;

        private bool rewind;

        private bool stoped;
    }
}
