using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core.Rendering;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Core
{
    public class TextureArray
    {
        public int Handle { get; set; }

        public int Dimension { get; set; }

        public int CurrentIndex = 0;
        public int Capacity = 12;
        public int MipMapLevels = 1;
        public bool HasMipMaps = false;


        public TextureArray(int size, int capacity = 12)
        {
            Capacity = capacity;
            Dimension = size;

            MipMapLevels = (int)Math.Floor(Math.Log(Dimension, 2)) + 1;

            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, MipMapLevels, SizedInternalFormat.Rgba8, Dimension, Dimension, Capacity);

            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
        }

    }
}
