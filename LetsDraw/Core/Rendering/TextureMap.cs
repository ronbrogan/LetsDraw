using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Data.Enums;
using LetsDraw.Loaders;
using OpenTK;

namespace LetsDraw.Core.Rendering
{
    public class TextureMap
    {
        public uint TextureBinding { get; private set; }

        public string TexturePath { get; set; }

        public Vector3 OriginOffset { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Turbulence { get; set; }

        public float BumpMultiplier { get; set; }

        public ImfChanOption ChannelOption { get; set; }

        public TextureMap(string texPath)
        {
            TextureBinding = TextureLoader.LoadTexture(texPath);
            TexturePath = texPath;
        }

        public uint RegenerateTexture(string path = null)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                TexturePath = path;
            }
            TextureBinding = TextureLoader.LoadTexture(TexturePath);
            return TextureBinding;
        }
    }
}
