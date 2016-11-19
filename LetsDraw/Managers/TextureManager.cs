using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Managers
{
    public static class TextureManager
    {
        public static Dictionary<Tuple<int, int>, uint> StagedTextures = new Dictionary<Tuple<int, int>, uint>();

        public static void StageTexture(string path)
        {
            var fullPath = Path.GetFullPath(path);

            var bmp = new Bitmap(fullPath);

        }
    }
}
