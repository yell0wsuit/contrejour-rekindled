using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace Mokus2D.Localization
{
    public static class LocalizedMessages
    {
        public static string GetString(string key, CultureInfo culture)
        {
            return strings.TryGetValue(key, out string value) ? value : null;
        }

        private static readonly Dictionary<string, string> strings = LoadStrings();

        private static Dictionary<string, string> LoadStrings()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "content", "strings.json");
            if (!File.Exists(path))
            {
                return [];
            }
            try
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                    ?? [];
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}
