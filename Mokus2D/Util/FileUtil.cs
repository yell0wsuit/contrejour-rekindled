using System.IO;

using Microsoft.Xna.Framework;

namespace Mokus2D.Util
{
    public class FileUtil
    {
        public static Stream OpenFile(string path)
        {
            return TitleContainer.OpenStream(path);
        }
    }
}
