using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace LetsDraw.Managers
{
    public static class TextureManager
    {
        public static int CurrentArray = -1;
        public static List<TextureArray> Arrays = new List<TextureArray>();

        public static void AddTexture(string path, TextureMap map)
        {
            var fullPath = Path.GetFullPath(path);

            var bmp = new Bitmap(fullPath);
            var dimension = Math.Max(bmp.Width, bmp.Height);

            if (Arrays.Where(a => a.CurrentIndex != a.Capacity).All(a => a.Dimension != dimension))
            {
                var newarray = new TextureArray(dimension);
                Arrays.Add(newarray);
            }

            var array = Arrays.First(a => a.CurrentIndex != a.Capacity && a.Dimension == dimension);

            AddTextureToArray(array, bmp, map);

        }

        public static void AddTextureToArray(TextureArray array, Bitmap bmp, TextureMap map)
        {
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2DArray, array.Handle);
            GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, array.CurrentIndex, bmp.Width, bmp.Height, 1, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            map.TextureArray = array.Handle;
            map.ArrayIndex = array.CurrentIndex;
            array.CurrentIndex++;
        }

        public static bool SetActiveTexture(int ArrayHandle)
        {
            if (CurrentArray != ArrayHandle)
            {
                var array = Arrays.First(a => a.Handle == ArrayHandle);
                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture2DArray, ArrayHandle);

                if (!array.HasMipMaps)
                {
                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
                    array.HasMipMaps = true;
                }
                CurrentArray = ArrayHandle;
                return true;
            }
            return false;
        }
    }
}
