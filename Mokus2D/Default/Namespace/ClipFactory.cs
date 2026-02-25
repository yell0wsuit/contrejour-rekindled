using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D;
using Mokus2D.Content;
using Mokus2D.Util;
using Mokus2D.Util.Xml;
using Mokus2D.Visual;
using Mokus2D.Visual.Data;

namespace Mokus2D.Default.Namespace
{
    public class ClipFactory
    {
        public static ClipFactory Instance { get; } = new();

        public static StringDictionary FullPaths { get; } = new();

        public static CGSize GetNodeSize(Sprite node)
        {
            ClipData nodeConfig = GetNodeConfig(node);
            return new CGSize(nodeConfig.Width, nodeConfig.Height);
        }

        private static ClipData GetNodeConfig(Sprite node)
        {
            return node.Config;
        }

        private static string CorrectName(string name)
        {
            string[] array = name.Split(['/', '\\']);
            string text = array[array.Length - 1];
            return text.Split(['.'])[0];
        }

        static ClipFactory()
        {
            serializer.AddAlias("NSMutableDictionary", typeof(ClipData).FullName);
            serializer.AddAlias("FlashPoint", typeof(Vector2).AssemblyQualifiedName);
            serializer.AddAlias("x", "X");
            serializer.AddAlias("y", "Y");
            serializer.AddAlias("useSheet", "UseSheet");
            serializer.AddAlias("width", "Width");
            serializer.AddAlias("height", "Height");
            serializer.AddAlias("frames", "Frames");
            serializer.AddAlias("tileData", "TileData");
            serializer.AddAlias("anchor", "Anchor");
            serializer.AddAlias("jpg", "Jpg");
            content = Mokus2DGame.SharedContent;
        }

        public static void Cache(string name)
        {
            _ = GetAnchorConfig(name);
        }

        public static Sprite CreateWithAnchor(string name)
        {
            TextureData anchorConfig = GetAnchorConfig(name);
            return (anchorConfig.Config.Frames > 1) ? new MovieClip(anchorConfig) : new Sprite(anchorConfig);
        }

        public static TextureData GetAnchorConfig(string name)
        {
            name = CorrectName(name);
            ClipData config = GetConfig(name);
            return new TextureData(content.Load<Texture2D>(clip_root + name), config);
        }

        public static Texture2D CreateWithoutConfig(string name)
        {
            return content.Load<Texture2D>(GetTexturePath(name));
        }

        public static string GetTexturePath(string name)
        {
            return clip_root + CorrectName(name);
        }

        public static ClipData GetConfig(string name)
        {
            name = CorrectName(name);
            ClipData clipData;
            if (!configsCache.ContainsKey(name))
            {
                string configPath = GetConfigPath(name);
                try
                {
                    using (Stream stream = FileUtil.OpenFile(configPath))
                    {
                        clipData = (ClipData)serializer.DeserializeFile(stream);
                        clipData.Initialize();
                        configsCache[name] = clipData;
                    }
                    return clipData;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            clipData = configsCache[name];
            return clipData;
        }

        private static string GetConfigPath(string name)
        {
            return Path.ChangeExtension(Path.Combine(content.RootDirectory, clip_root + name), "xml");
        }

        private static string PathForResourceOfType(string name)
        {
            return name;
        }

        public static bool HasConfig(string name)
        {
            return GetConfig(name) != null;
        }

        public static Rectangle GetBounds(ISizeNode node)
        {
            float num = 0f;
            float num2 = 0f;
            Vector2 size = node.Size;
            if (node is IAnchorNode)
            {
                IAnchorNode anchorNode = (IAnchorNode)node;
                num = -anchorNode.Anchor.X * size.X;
                num2 = (anchorNode.Anchor.Y - 1f) * size.Y;
            }
            return new Rectangle((int)num, (int)num2, (int)size.X, (int)size.Y);
        }

        public static LevelBuilderBase debug_builder;
        private static readonly string clip_root = "mc/hd/";

        private static readonly XmlSerializer serializer = new();

        public static ReferenceCountingContentManager content;

        private static readonly Dictionary<string, ClipData> configsCache = new();
        private static readonly Dictionary<object, Dictionary<int, MovieClip>> debugPoints = new();
    }
}
