using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Managers
{
    public static class TextureManager
    {
        public static int CurrentHandle = -1;

        public static Dictionary<Tuple<int, int>, uint> StagedTextures = new Dictionary<Tuple<int, int>, uint>();

        public static void StageTexture(string path)
        {
            var fullPath = Path.GetFullPath(path);

            var bmp = new Bitmap(fullPath);

        }

        public static bool SetActiveTexture(int TextureHandle, TextureUnit unit)
        {
            if (CurrentHandle != TextureHandle)
            {
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.Texture2D, TextureHandle);

                CurrentHandle = TextureHandle;
                return true;
            }
            return false;
        }
    }
}
