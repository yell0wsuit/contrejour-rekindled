using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Mokus2D.Default.Namespace
{
    public class TexturedNeck : PrimitivesNode
    {
        public TexturedNeck(string textureFile)
        {
            Texture = ClipFactory.CreateWithoutConfig(textureFile);
        }

        public void AddPoint(Vector2 point)
        {
            vertices.Add(point);
            RefreshTextureCoords();
        }

        public void RefreshTextureCoords()
        {
            for (int i = textureCoords.Count; i < vertices.Count; i++)
            {
                Vector2 vector = new(i / 2, i % 2);
                textureCoords.Add(vector);
            }
        }

        public void ClearVertices()
        {
            vertices.Clear();
        }

        public void Clear()
        {
            vertices.Clear();
            textureCoords.Clear();
        }

        protected override void DrawPrimitives()
        {
            if (vertices.Count > 2)
            {
                GraphUtil.FillTrianglesStripTextureCoordsTexture(vertices, textureCoords, Texture);
            }
        }

        protected List<Vector2> vertices = new();

        protected List<Vector2> textureCoords = new();
    }
}
