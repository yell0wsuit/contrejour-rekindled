using System.Collections.Generic;
using System.IO;

using Mokus2D.Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Util;
using Mokus2D.Util.Xml;

namespace Mokus2D.Content
{
    public class LevelsCache
    {
        public Dictionary<string, Level> CachedLevels { get; } = new();

        private string correctName(string name)
        {
            string[] array = name.Split(['/', '\\']);
            return array[array.Length - 1];
        }

        public LevelsCache()
        {
            serializer.AddAlias("Level", typeof(Level).AssemblyQualifiedName);
            serializer.AddAlias("NSMutableArray", typeof(ArrayList).FullName);
            serializer.AddAlias("NSMutableDictionary", typeof(Hashtable).FullName);
            serializer.AddAlias("FlashPoint", typeof(Vector2).AssemblyQualifiedName);
            serializer.AddAlias("x", "X");
            serializer.AddAlias("y", "Y");
            serializer.AddAlias("width", "Width");
            serializer.AddAlias("height", "Height");
            serializer.AddAlias("frames", "Frames");
            serializer.AddAlias("tileData", "TileData");
            serializer.AddAlias("anchor", "Anchor");
            serializer.AddAlias("useSheet", "UseSheet");
            content = Mokus2DGame.SharedContent;
        }

        public Level Load(string name)
        {
            name = correctName(name);
            Level level;
            if (!CachedLevels.ContainsKey(name))
            {
                string text = Path.ChangeExtension(Path.Combine(content.RootDirectory, clip_root + name), "xml");
                Stream stream = FileUtil.OpenFile(text);
                level = (Level)serializer.DeserializeFile(stream);
                CachedLevels[name] = level;
            }
            else
            {
                level = CachedLevels[name];
            }
            return level;
        }

        private readonly string clip_root = "Levels/";

        private readonly XmlSerializer serializer = new();
        private readonly ReferenceCountingContentManager content;
    }
}
