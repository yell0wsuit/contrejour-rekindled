using System.Collections.Generic;

using Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.Visual.Data
{
    public class ClipData
    {
        public CGSize Size
        {
            get
            {
                return new CGSize(Width, Height);
            }
        }

        public void Initialize()
        {
            Anchor.Y = 1f - Anchor.Y;
            int num = 0;
            while (num < TileData.Y)
            {
                int num2 = 0;
                while (num2 < TileData.X)
                {
                    Rectangle rectangle = new(num2 * Width, num * Height, Width, Height);
                    FramesBounds.Add(rectangle);
                    num2++;
                }
                num++;
            }
        }

        public int Width;

        public int Height;

        public int Frames;

        public bool UseSheet;

        public Vector2 TileData;

        public Vector2 Anchor;

        public bool Jpg;

        public List<Rectangle> FramesBounds = new();
    }
}
