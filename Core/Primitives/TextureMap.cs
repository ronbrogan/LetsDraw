using Core.Rendering.Enums;
using System.Numerics;

namespace Core.Primitives
{
    public class TextureMap
    {
        public uint TextureArray { get; set; }
        public int ArrayIndex { get; set; }

        public int TextureBinding { get; private set; }

        public string TexturePath { get; set; }

        public Vector3 OriginOffset { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Turbulence { get; set; }

        public float BumpMultiplier { get; set; }

        public ImfChanOption ChannelOption { get; set; }

        // Empty constructor for serializer compat
        public TextureMap()
        {
            
        }

        public TextureMap(string texPath)
        { 
            // TODO invert this
            //TextureBinding = TextureLoader.LoadTexture(texPath);
            TexturePath = texPath;
        }

        public int GenerateTexture(string path = null)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                TexturePath = path;
            }
            // TODO invert this
            //TextureBinding = TextureLoader.LoadTexture(TexturePath);
            return TextureBinding;
        }
    }
}
