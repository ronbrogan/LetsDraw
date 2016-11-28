using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace LetsDraw.Loaders
{
    public static class TextureLoader
    {
        public static int LoadTexture(BitmapData data, int width, int height, PixelFormat inputFormat = PixelFormat.Format24bppRgb)
        {
            int textureObject;

            GL.GenTextures(1, out textureObject);
            GL.BindTexture(TextureTarget.Texture2D, textureObject);
            

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            var mipMapLevels = (int)Math.Floor(Math.Log(Math.Max(width, height), 2)) + 1;
            GL.TexStorage2D(TextureTarget2d.Texture2D, mipMapLevels, SizedInternalFormat.Rgba8, width, height);

            switch (inputFormat)
            {
                case PixelFormat.Format32bppArgb:
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                    break;
                
                case PixelFormat.Format24bppRgb:
                default:
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
                    break;
            }

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

            float maxAniso;
            GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAniso);
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAniso);

            var error1 = GL.GetError();
            if (error1 != ErrorCode.NoError)
            {
                Console.WriteLine("-- Error {0} occured at {1}", error1, "some place in texture loader");
            }

            return textureObject;
        }

        public static int LoadTexture(string filename)
        {

            var error1 = GL.GetError();
            if (error1 != ErrorCode.NoError)
            {
                Console.WriteLine("-- Error {0} occured at {1}", error1, "some place in the texture loader, beginning");
            }

            var fullPath = Path.GetFullPath(filename);

            var bmp = new Bitmap(fullPath);
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            error1 = GL.GetError();
            if (error1 != ErrorCode.NoError)
            {
                Console.WriteLine("-- Error {0} occured at {1}", error1, "some place in texture loader");
            }

            var texAddr = LoadTexture(data, bmp.Width, bmp.Height);
            bmp.UnlockBits(data);
            return texAddr;
        }

        
    }
}
