using System;

using Microsoft.Xna.Framework;

using Mokus2D.Util;
using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class MovieClip(TextureData config) : Sprite(config)
    {
        public bool Rewind { get; set; }

        public bool Stoped { get; set; }

        public uint TotalFrames => (uint)Config.Frames;

        public float MinFrame
        {
            get; set
            {
                field = value;
                if (CurrentFrame < field)
                {
                    CurrentFrame = field;
                }
            }
        }

        public float MaxFrame
        {
            get; set
            {
                field = value - 0.0001f;
                if (CurrentFrame > field)
                {
                    CurrentFrame = field;
                }
            }
        } = config.Config.Frames - 0.0001f;

        public float CurrentFrame
        {
            get; set
            {
                if ((value > MaxFrame || value < MinFrame) && !Stoped && !Repeat)
                {
                    if (value > MaxFrame)
                    {
                        value = MaxFrame;
                    }
                    if (value < MinFrame)
                    {
                        value = MinFrame;
                    }
                    Stoped = true;
                }
                while (value > MaxFrame)
                {
                    value = value - MaxFrame + MinFrame;
                }
                while (value < MinFrame)
                {
                    value = value - MinFrame + MaxFrame;
                }
                field = value;
                if ((uint)field != frameValue)
                {
                    frameValue = (uint)field;
                    currentFrameBounds = config.FramesBounds[(int)frameValue];
                }
            }
        }

        public event Action EndEvent;

        public override void Update(float time)
        {
            base.Update(time);
            if (!Stoped)
            {
                int num = Rewind ? (-1) : 1;
                float num2 = time * fps * Speed * num;
                float num3 = CurrentFrame + num2;
                CurrentFrame = num3;
                if ((num3 > MaxFrame && !Rewind) || (num3 < MinFrame && Rewind))
                {
                    EndEvent.Dispatch();
                }
            }
        }

        public void GotoAndPlay(uint frame)
        {
            Stoped = false;
            CurrentFrame = frame;
        }

        public void GotoAndStop(uint frame)
        {
            Stoped = true;
            CurrentFrame = frame;
        }

        protected override Rectangle? GetTileRectangle()
        {
            return new global::Microsoft.Xna.Framework.Rectangle?(currentFrameBounds);
        }

        private const float EPSILON = 0.0001f;

        public bool Repeat = true;

        public float Speed = 1f;
        private Rectangle currentFrameBounds = config.Config.FramesBounds[0];

        private readonly float fps = 30f;

        private uint frameValue = 1U;
    }
}
