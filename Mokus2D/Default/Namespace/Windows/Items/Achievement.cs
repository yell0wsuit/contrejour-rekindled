using System;
using System.Collections.Generic;
using System.IO;

namespace Default.Namespace.Windows.Items
{
    public sealed class Achievement
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int GamerScore { get; set; }

        public bool IsEarned { get; set; }

        public Stream GetPicture()
        {
            return new MemoryStream(EmptyPng, writable: false);
        }

        private static readonly byte[] EmptyPng = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAusB9Ywdr2gAAAAASUVORK5CYII=");
    }

    public sealed class AchievementCollection : List<Achievement>
    {
    }
}
