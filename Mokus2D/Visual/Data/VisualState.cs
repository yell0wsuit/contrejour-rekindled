using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Visual.Util;

namespace Mokus2D.Visual.Data
{
    public class VisualState
    {
        public bool TransformationDirty
        {
            get => transformationDirty; set => transformationDirty = value;
        }

        public Vector2 SpritesScaleFactor
        {
            get => spritesScaleFactor; set => spritesScaleFactor = value;
        }

        public Color Color => color;

        public float Opacity
        {
            get => opacity; set => opacity = value;
        }

        public Matrix Matrix => matrix;

        public Matrix PrimitivesMatrix => Matrix * primitivesToScreenMatrix;

        public VisualState(int width, int height)
            : this(width, height, Vector2.One)
        {
        }

        public VisualState(int width, int height, Vector2 scaleFactor)
        {
            matrix = Matrix.Identity;
            opacity = 1f;
            spritesScaleFactor = Vector2.One;
            color = Color.White;
            transformationDirty = false;
            spritesScaleFactor = scaleFactor;
            primitivesToScreenMatrix = PrimitivesDrawing.CreatePrimitivesMatrix(new Vector2(width, height));
        }

        public VisualState(VisualState parent)
        {
            matrix = Matrix.Identity;
            opacity = 1f;
            spritesScaleFactor = Vector2.One;
            color = Color.White;
            transformationDirty = false;
            spritesScaleFactor = parent.spritesScaleFactor;
            primitivesToScreenMatrix = parent.primitivesToScreenMatrix;
        }

        public void Refresh(VisualState parentState, Matrix matrix, float nodeOpacity, Color nodeColor, bool ignoreParentOpacity, bool ignoreParentColor)
        {
            this.matrix = matrix * parentState.Matrix;
            RefreshValues(parentState, nodeOpacity, nodeColor, ignoreParentOpacity, ignoreParentColor);
            transformationDirty = true;
        }

        public void RefreshValues(VisualState parentState, float nodeOpacity, Color nodeColor, bool ignoreParentOpacity, bool ignoreParentColor)
        {
            opacity = (ignoreParentOpacity ? nodeOpacity : (nodeOpacity * parentState.opacity));
            if (!ignoreParentColor)
            {
                color = nodeColor.Mult(parentState.color);
            }
            color.A = byte.MaxValue;
            spritesScaleFactor = parentState.spritesScaleFactor;
        }

        private Matrix matrix;

        private Matrix primitivesToScreenMatrix;

        private float opacity;

        private bool transformationDirty;

        private Vector2 spritesScaleFactor;

        private Color color;
    }
}
