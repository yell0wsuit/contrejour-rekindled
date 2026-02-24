using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual.Util
{
    public struct PrimitivesDrawing : IDisposable
    {
        public static Matrix CreatePrimitivesMatrix(Vector2 size)
        {
            return Matrix.CreateTranslation(-size.X / 2f, -size.Y / 2f, 0f) * Matrix.CreateRotationX(MathHelper.ToRadians(180f)) * Matrix.CreateScale(2f / size.X, 2f / size.Y, 0f);
        }

        public static void RefreshGraphicsDevice(GraphicsDevice device)
        {
            Effect = new BasicEffect(device);
        }

        public PrimitivesDrawing(VisualState state)
        {
            this = new PrimitivesDrawing(state, state.Matrix, null);
        }

        public PrimitivesDrawing(VisualState state, Matrix matrix, Texture2D texture = null)
        {
            GraphUtil.BeginDrawPrimitives(state, matrix, texture);
        }

        public readonly void Dispose()
        {
            GraphUtil.EndDrawPrimitives();
        }

        public static BasicEffect Effect;
    }
}
