using System;
using System.Collections.Generic;

using Default.Namespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual.Data;

namespace Mokus2D.Visual.Util
{
    public static class GraphUtil
    {
        public static void BeginDrawPrimitives(VisualState state)
        {
            BeginDrawPrimitives(state, state.Matrix);
        }

        public static void BeginDrawPrimitives(VisualState state, Matrix matrix, Texture2D texture)
        {
            Mokus2DGame.Device.SamplerStates[0] = SamplerState.LinearWrap;
            Mokus2DGame.Device.BlendState = BlendState.NonPremultiplied;
            PrimitivesDrawing.Effect.Projection = matrix;
            PrimitivesDrawing.Effect.Alpha = state.Opacity;
            PrimitivesDrawing.Effect.VertexColorEnabled = true;
            PrimitivesDrawing.Effect.TextureEnabled = texture != null;
            PrimitivesDrawing.Effect.Texture = texture;
            foreach (EffectPass effectPass in PrimitivesDrawing.Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
            }
        }

        public static void BeginDrawPrimitives(VisualState state, Matrix matrix)
        {
            BeginDrawPrimitives(state, matrix, null);
        }

        public static void EndDrawPrimitives()
        {
        }

        public static void DrawTriangleFan(VertexPositionColor[] vertices)
        {
            short[] array = CreateTriangleFanIndices((short)vertices.Length);
            DrawTriangleList(vertices, array);
        }

        public static void DrawTriangleStrip(VertexPositionColor[] vertices)
        {
            if (vertices.Length < 3)
            {
                return;
            }
            Mokus2DGame.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length - 2, VertexPositionColor.VertexDeclaration);
        }

        public static void DrawTriangleList(VertexPositionColor[] vertices, short[] indices)
        {
            Mokus2DGame.Device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColor.VertexDeclaration);
        }

        public static void DrawTriangleList(VertexPositionColor[] vertices)
        {
            Mokus2DGame.Device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3, VertexPositionColor.VertexDeclaration);
        }

        public static void DrawLineList(VertexPositionColor[] vertices)
        {
            Mokus2DGame.Device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length / 2, VertexPositionColor.VertexDeclaration);
        }

        public static void DrawLineStrip(GraphicsDevice device, VertexPositionColor[] vertices)
        {
            device.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1, VertexPositionColor.VertexDeclaration);
        }

        public static Vector2 StringToVector(string source)
        {
            string[] array = source.Split([',']);
            return new Vector2((float)Convert.ToDouble(array[0]), (float)Convert.ToDouble(array[1]));
        }

        public static void DrawRectangle(GraphicsDevice device, float x, float y, float width, float height, Color color)
        {
            DrawLineStrip(device,
            [
                new VertexPositionColor(new Vector3(x, y, 0f), color),
                new VertexPositionColor(new Vector3(x + width - 1f, y, 0f), color),
                new VertexPositionColor(new Vector3(x + width - 1f, y + height - 1f, 0f), color),
                new VertexPositionColor(new Vector3(x, y + height - 1f, 0f), color),
                new VertexPositionColor(new Vector3(x, y, 0f), color)
            ]);
        }

        public static VertexPositionColor[] getVertexPositionColor(List<Vector2> polygon, List<Color> colors, int lenght = -1)
        {
            if (lenght == -1)
            {
                lenght = polygon.Count;
            }
            VertexPositionColor[] array = new VertexPositionColor[lenght];
            for (int i = 0; i < lenght; i++)
            {
                array[i].Position = polygon[i].ToVector3();
                array[i].Color = colors[i];
            }
            return array;
        }

        public static VertexPositionColor[] getVertexPositionColor(List<Vector2> polygon, Color color, int lenght = -1)
        {
            if (lenght == -1)
            {
                lenght = polygon.Count;
            }
            VertexPositionColor[] array = new VertexPositionColor[lenght];
            for (int i = 0; i < lenght; i++)
            {
                array[i].Position = polygon[i].ToVector3();
                array[i].Color = color;
            }
            return array;
        }

        public static short[] CreateTriangleFanIndices(short count)
        {
            short[] array = new short[((count - 3) * 3 + 3)];
            for (short num = 0; num < count - 2; num += 1)
            {
                short num2 = (short)(num * 3);
                array[num2] = 0;
                array[num2 + 1] = (short)(num + 1);
                array[num2 + 2] = (short)(num + 2);
            }
            return array;
        }

        public static float GetRootRotationRadians(Node node)
        {
            float num = 0f;
            while (node != null)
            {
                num += node.RotationRadians;
                node = node.Parent;
            }
            return num;
        }

        public static Vector2 GetOutVertex(Vector2 inVertex, Vector2 nextInVertex, Vector2 controlVertex, float width)
        {
            Vector2 vector = controlVertex - inVertex;
            float num = (float)Math.Atan2(vector.Y, vector.X);
            num -= 1.5707964f;
            return nextInVertex + VectorUtil.ToVector(width, num);
        }

        public static void CreateBezierPoints(IList<Vector2> polygon, int segments, IList<Vector2> result)
        {
            for (int i = 0; i < polygon.Count - 1; i += 2)
            {
                int num = ((i == polygon.Count - 2) ? 0 : (i + 2));
                GetBezierPoints(polygon[i], polygon[i + 1], polygon[num], segments, false, i, result);
            }
        }

        public static void GetBezierPoints(Vector2 origin, Vector2 control, Vector2 destination, int segments, bool insertLast, int index, IList<Vector2> result)
        {
            float num = 0f;
            for (int i = 0; i < segments; i++)
            {
                float num2 = (float)Math.Pow((double)(1f - num), 2.0) * origin.X + 2f * (1f - num) * num * control.X + num * num * destination.X;
                float num3 = (float)Math.Pow((double)(1f - num), 2.0) * origin.Y + 2f * (1f - num) * num * control.Y + num * num * destination.Y;
                result[index + i] = new Vector2(num2, num3);
                num += 1f / segments;
            }
            if (insertLast)
            {
                result[index + segments] = destination;
            }
        }

        public static void CreateGradientBorderColors(VertexPositionColor[] vertices, Color inColor)
        {
            Color color = inColor;
            color.A = 0;
            for (int i = 0; i < vertices.Length / 6; i++)
            {
                int num = i * 6;
                vertices[num].Color = inColor;
                vertices[num + 1].Color = inColor;
                vertices[num + 2].Color = color;
                vertices[num + 3].Color = inColor;
                vertices[num + 4].Color = color;
                vertices[num + 5].Color = color;
            }
        }

        public static void CreateGradientBorder(List<Vector2> surface, float width, VertexPositionColor[] vertices)
        {
            Vector2 vector = surface[0];
            Vector2 vector2 = GetOutVertex(surface[surface.Count - 1], vector, surface[1], width);
            for (int i = 0; i < surface.Count; i++)
            {
                Vector2 vector3 = surface[(i + 1) % surface.Count];
                Vector2 vector4 = surface[(i + 2) % surface.Count];
                Vector2 outVertex = GetOutVertex(vector, vector3, vector4, width);
                int num = i * 6;
                vertices[num].Position = vector.ToVector3();
                vertices[num + 1].Position = vector3.ToVector3();
                vertices[num + 2].Position = vector2.ToVector3();
                vertices[num + 3].Position = vector3.ToVector3();
                vertices[num + 4].Position = vector2.ToVector3();
                vertices[num + 5].Position = outVertex.ToVector3();
                vector = vector3;
                vector2 = outVertex;
            }
        }

        public static void CreateGradientColorsStripStartColorEndColor(List<Color> colors, Color startColor, Color endColor)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                colors[i] = ((i % 2 != 0) ? endColor : startColor);
            }
        }

        public static void SetColor(VertexPositionColor[] vertices, Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = color;
            }
        }

        public static void SetColor(VertexPositionColorTexture[] vertices, Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = color;
            }
        }

        public static void SetGradientColorsStrip(Color startColor, Color endColor, VertexPositionColorTexture[] colors)
        {
            for (int i = 0; i < colors.Length; i += 2)
            {
                colors[i].Color = startColor;
                colors[i + 1].Color = endColor;
            }
        }

        public static void CreateGradientColorsList(int surfaceSize, Color startColor, Color endColor, VertexPositionColorTexture[] colors)
        {
            int num = colors.Length;
            int num2 = surfaceSize * 6;
            for (int i = 0; i < surfaceSize; i++)
            {
                int num3 = i * 6;
                colors[num3].Color = startColor;
                colors[num3 + 1].Color = startColor;
                colors[num3 + 2].Color = endColor;
                colors[num3 + 3].Color = startColor;
                colors[num3 + 4].Color = endColor;
                colors[num3 + 5].Color = endColor;
            }
        }

        public static void CreateGradientColors(int start, int end, Color fromColor, Color toColor, Color outColor, VertexPositionColorTexture[] colors)
        {
            Color color = fromColor;
            Extensions.ColorDiff colorDiff = toColor.Sub(fromColor) * (1f / (end - (float)start));
            Color color2 = color.Add(colorDiff);
            for (int i = start; i < end; i++)
            {
                int num = i * 6;
                colors[num].Color = color;
                colors[num + 1].Color = color2;
                colors[num + 2].Color = outColor;
                colors[num + 3].Color = color2;
                colors[num + 4].Color = outColor;
                colors[num + 5].Color = outColor;
                color = color2;
                color2 = color2.Add(colorDiff);
            }
        }

        public static void CreateGradientColorsForPolygonsStartEndStartColorEndColorColorsVector(int start, int end, Color startColor, Color endColor, List<List<Color>> colors)
        {
            if (colors.Count < end)
            {
                while (colors.Count < end)
                {
                    colors.Add(new List<Color>(4));
                }
            }
            Color color = startColor;
            Extensions.ColorDiff colorDiff = endColor.Sub(startColor) * (1f / (end - (float)start));
            Color color2 = color.Add(colorDiff);
            for (int i = start; i < end; i++)
            {
                List<Color> list = colors[i];
                if (list == null)
                {
                    list = new List<Color>(4);
                    colors[i] = list;
                }
                while (list.Count < 4)
                {
                    list.Add(default(Color));
                }
                list[0] = color;
                list[1] = color2;
                list[2] = color2;
                list[3] = color;
                color = color2;
                color2 = color2.Add(colorDiff);
            }
        }

        public static void CreateGradientColorsForPolygonsStartColorEndColorColorsVector(int polygonCount, Color startColor, Color endColor, List<List<Color>> colors)
        {
            CreateGradientColorsForPolygonsStartEndStartColorEndColorColorsVector(0, polygonCount, startColor, endColor, colors);
        }

        public static void FillTriangles(List<Vector2> triangles)
        {
            FillTrianglesTrianglesSize(triangles, triangles.Count);
        }

        public static void GetCircleRadiusSegmentsResult(Vector2 center, float radius, int segments, ref VertexPositionColorTexture[] result)
        {
            float num = 6.2831855f / segments;
            float num2 = 0f;
            for (int i = 0; i < segments; i++)
            {
                result[i].Position = new Vector3(radius * Maths.Cos(num2) + center.X, radius * Maths.Sin(num2) + center.Y, 0f);
                num2 += num;
            }
        }

        public static void FillConvexColors(List<Vector2> polygon, List<Color> colors)
        {
            DrawTriangleFan(getVertexPositionColor(polygon, colors, -1));
        }

        public static void FillConvexColor(List<Vector2> polygon, Color color)
        {
            DrawTriangleFan(getVertexPositionColor(polygon, color, -1));
        }

        public static void FillTrianglesStripTextureCoordsTextureColor(List<Vector2> triangles, List<Vector2> textureCoords, Texture2D texture, Color color)
        {
            FillTrianglesTextureCoordsTextureLoopTypeColor(triangles, textureCoords, texture, PrimitiveType.TriangleStrip, color);
        }

        public static void FillTrianglesStrip(VertexPositionColorTexture[] vertices, Texture2D texture = null)
        {
            Mokus2DGame.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length - 2);
        }

        public static void FillTrianglesList(VertexPositionColorTexture[] vertices, Texture2D texture = null)
        {
            Mokus2DGame.Device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
        }

        public static void FillTrianglesTextureCoordsTextureColor(List<Vector2> triangles, List<Vector2> textureCoords, Texture2D texture, Color color)
        {
            FillTrianglesTextureCoordsTextureLoopTypeColor(triangles, textureCoords, texture, PrimitiveType.TriangleList, color);
        }

        public static void FillTrianglesStripColors(List<Vector2> triangles, List<Color> colors)
        {
            FillTrianglesColorsLoopType(triangles, colors, PrimitiveType.TriangleStrip);
        }

        public static void FillTrianglesStripTextureCoordsTexture(List<Vector2> triangles, List<Vector2> textureCoords, Texture2D texture)
        {
            FillTrianglesTextureCoordsTextureLoopType(triangles, textureCoords, texture, PrimitiveType.TriangleStrip);
        }

        public static void FillTrianglesTextureCoordsTexture(List<Vector2> vertices, List<Vector2> textureCoords, Texture2D texture)
        {
            FillTrianglesTextureCoordsTextureLoopType(vertices, textureCoords, texture, PrimitiveType.TriangleList);
        }

        public static void FillTrianglesTextureCoordsTextureLoopType(List<Vector2> vertices, List<Vector2> textureCoords, Texture2D texture, PrimitiveType loopType)
        {
            FillTrianglesTextureCoordsTextureLoopTypeColor(vertices, textureCoords, texture, loopType, new Color(255, 255, 255, 255));
        }

        public static void FillTrianglesTextureCoordsTextureLoopTypeColor(List<Vector2> vertices, List<Vector2> textureCoords, Texture2D texture, PrimitiveType loopType, Color color)
        {
            VertexPositionColorTexture[] array = new VertexPositionColorTexture[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                array[i].Position = vertices[i].ToVector3();
                array[i].Color = color;
                array[i].TextureCoordinate = textureCoords[i];
            }
            int num = ((loopType == PrimitiveType.TriangleList) ? (vertices.Count / 3) : (vertices.Count - 2));
            Mokus2DGame.Device.DrawUserPrimitives(loopType, array, 0, num, VertexPositionColorTexture.VertexDeclaration);
        }

        public static void FillTrianglesColorsLoopType(List<Vector2> vertices, List<Color> colors, PrimitiveType loopType)
        {
            VertexPositionColor[] vertexPositionColor = getVertexPositionColor(vertices, colors, -1);
            DrawTriangleStrip(vertexPositionColor);
        }

        public static void FillTrianglesColors(List<Vector2> vertices, List<Color> colors)
        {
            FillTrianglesColorsLoopType(vertices, colors, PrimitiveType.TriangleList);
        }

        public static void FillConvex(List<Vector2> polygon)
        {
            FillConvexColor(polygon, new Color(0, 0, 0, 255));
        }

        public static void FillPolygonsColorsIteratorSize(List<List<Vector2>> points, List<List<Color>> colors, int size)
        {
            foreach (List<Color> list in colors)
            {
                for (int i = 0; i < size; i++)
                {
                    List<Vector2> list2 = points[i];
                    FillConvexColors(points[i], list);
                }
            }
        }

        public static void FillPolygonsColorsSize(List<List<Vector2>> points, List<List<Color>> colors, int size)
        {
            for (int i = 0; i < size; i++)
            {
                List<Vector2> list = points[i];
                List<Color> list2 = colors[i];
                FillConvexColors(list, list2);
            }
        }

        public static void FillPolygonsColors(List<List<Vector2>> points, List<List<Color>> colors)
        {
            FillPolygonsColorsSize(points, colors, points.Count);
        }

        public static void CreateTextureCoordsVerticesStep(int size, VertexPositionColorTexture[] vertices, float step)
        {
            for (int i = 0; i < size; i++)
            {
                int num = i * 6;
                float num2 = i * step;
                float num3 = num2 + step;
                vertices[num].TextureCoordinate = new Vector2(num2, 0f);
                vertices[num + 1].TextureCoordinate = new Vector2(num3, 0f);
                vertices[num + 2].TextureCoordinate = new Vector2(num2, 1f);
                vertices[num + 3].TextureCoordinate = new Vector2(num2, 1f);
                vertices[num + 4].TextureCoordinate = new Vector2(num3, 1f);
                vertices[num + 5].TextureCoordinate = new Vector2(num3, 0f);
                for (int j = 0; j < 6; j++)
                {
                    vertices[num + j].Color = Color.White;
                }
            }
        }

        public static void CreateTextureCoordsVertices(int size, VertexPositionColorTexture[] vertices)
        {
            CreateTextureCoordsVerticesStep(size, vertices, 1f);
        }

        public static void CreateBorderTextureCoordsTextureWidthVertices(List<Vector2> surface, float textureWidth, ref List<Vector2> vertices)
        {
            float num = 0f;
            for (int i = 0; i < surface.Count; i++)
            {
                Vector2 vector = surface[i];
                Vector2 vector2 = surface[(i + 1) % surface.Count];
                float num2 = (vector2 - vector).Length() / textureWidth + num;
                int num3 = i * 6;
                vertices[num3] = new Vector2(0f, 0f);
                vertices[num3 + 1] = new Vector2(1f, 0f);
                vertices[num3 + 2] = new Vector2(0f, 1f);
                vertices[num3 + 3] = new Vector2(0f, 1f);
                vertices[num3 + 4] = new Vector2(1f, 1f);
                vertices[num3 + 5] = new Vector2(1f, 0f);
                num = num2 - (int)num2;
            }
        }

        public static void CreateGradientBorderWidthVertices(IList<Vector2> surface, float width, VertexPositionColorTexture[] vertices)
        {
            Vector2 vector = surface[0];
            Vector2 vector2 = GetOutVertex(surface[surface.Count - 1], vector, surface[1], width);
            for (int i = 0; i < surface.Count; i++)
            {
                Vector2 vector3 = surface[(i + 1) % surface.Count];
                Vector2 vector4 = surface[(i + 2) % surface.Count];
                Vector2 outVertex = GetOutVertex(vector, vector3, vector4, width);
                int num = i * 6;
                vertices[num].Position = vector.ToVector3();
                vertices[num + 1].Position = vector3.ToVector3();
                vertices[num + 2].Position = vector2.ToVector3();
                vertices[num + 3].Position = vector3.ToVector3();
                vertices[num + 4].Position = vector2.ToVector3();
                vertices[num + 5].Position = outVertex.ToVector3();
                vector = vector3;
                vector2 = outVertex;
            }
        }

        public static void FillTrianglesColor(List<Vector2> triangles, Color color)
        {
            FillTrianglesTrianglesSizeColorLoopType(triangles, triangles.Count, color, PrimitiveType.TriangleList);
        }

        public static void FillTrianglesStripColor(List<Vector2> triangles, Color color)
        {
            FillTrianglesTrianglesSizeColorLoopType(triangles, triangles.Count, color, PrimitiveType.TriangleStrip);
        }

        public static void FillTrianglesTrianglesSizeColor(List<Vector2> triangles, int trianglesSize, Color color)
        {
            FillTrianglesTrianglesSizeColorLoopType(triangles, trianglesSize, color, PrimitiveType.TriangleList);
        }

        public static void FillTrianglesTrianglesSizeColorLoopType(List<Vector2> triangles, int trianglesSize, Color color, PrimitiveType loopType)
        {
            DrawTriangleStrip(getVertexPositionColor(triangles, color, trianglesSize));
        }

        public static void FillTrianglesTrianglesSize(List<Vector2> triangles, int trianglesSize)
        {
            FillTrianglesTrianglesSizeColor(triangles, trianglesSize, new Color(0, 0, 0, 255));
        }

        public static List<Vector2> LineToNeckNeckWidth(List<Vector2> line, float width)
        {
            List<Vector2> list = new();
            Pair<Vector2> pair;
            for (int i = 0; i < line.Count - 1; i++)
            {
                pair = ContreDrawUtil.GetPointsPair(line[i], line[i], line[i + 1], width);
                list.Add(pair.First);
                list.Add(pair.Second);
            }
            pair = ContreDrawUtil.GetPointsPair(line[line.Count - 1], line[line.Count - 1], line[line.Count - 2], width);
            list.Add(pair.Second);
            list.Add(pair.First);
            return list;
        }

        public static List<Vector2> CreateBezierLineBezierMaxBezierStep(List<Vector2> line, float maxStep)
        {
            List<Vector2> list = new();
            list.Add(line[0]);
            for (int i = 1; i < line.Count - 1; i++)
            {
                Vector2 vector = line[i - 1].Middle(line[i]);
                Vector2 vector2 = line[i].Middle(line[i + 1]);
                int num = (int)Math.Ceiling((double)((vector.DistanceTo(line[i]) + vector2.DistanceTo(line[i])) / maxStep));
                if (num <= 1)
                {
                    list.Add(vector);
                }
                else
                {
                    bool flag = i == line.Count - 1;
                    Maths.GetBezierPointsControlDestinationSegmentsInsertLastResult(vector, line[i], vector2, num, flag, list);
                }
            }
            list.Add(line[line.Count - 1]);
            return list;
        }

        public static void CreateBezierSurfaceSurfaceSegments(List<Vector2> polygon, ref List<Vector2> surface, int segments)
        {
            Vector2 vector = (polygon[0] + polygon[1]) * 0.5f;
            for (int i = 0; i < polygon.Count; i++)
            {
                Vector2 vector2 = vector;
                Vector2 vector3 = polygon[(i + 1) % polygon.Count];
                vector = (vector3 + polygon[(i + 2) % polygon.Count]) * 0.5f;
                Maths.GetBezierPointsControlDestinationSegmentsInsertLastResult(vector2, vector3, vector, segments, false, surface);
            }
        }

        public static void CreateBezierPointsSurfaceSegments(List<Vector2> polygon, ref List<Vector2> surface, int segments)
        {
            for (int i = 0; i < polygon.Count - 1; i += 2)
            {
                int num = ((i == polygon.Count - 2) ? 0 : (i + 2));
                Maths.GetBezierPointsControlDestinationSegmentsInsertLastResult(polygon[i], polygon[i + 1], polygon[num], segments, false, surface);
            }
        }
    }
}
