using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Visual
{
    public class RenderSprite : Sprite
    {
        public RenderSprite(Vector2 size)
            : base(new RenderTarget2D(Mokus2DGame.Device, (int)size.X, (int)size.Y))
        {
            renderRoot = new RootNode((int)size.X, (int)size.Y);
            renderTarget = (RenderTarget2D)texture;
        }

        public void RedrawTexture()
        {
            renderRoot.ScaleX = Math.Sign(Root.ScaleX);
            renderRoot.ScaleY = Math.Sign(Root.ScaleY);
            renderRoot.SpritesScaleFactor = Root.SpritesScaleFactor;
            renderRoot.Position = AnchorInPixels;
            Mokus2DGame.Device.SetRenderTarget(renderTarget);
            Mokus2DGame.Device.Clear(Color.Green * 0f);
            DrawContent();
            Mokus2DGame.Device.SetRenderTarget(null);
        }

        protected virtual void DrawContent()
        {
            renderRoot.DrawAll();
        }

        public override void AddChild(Node node, int nodeLayer)
        {
            renderRoot.AddChild(node, nodeLayer);
        }

        protected readonly RootNode renderRoot;

        private RenderTarget2D renderTarget;
    }
}
