using Core.Data.Enums;
using Core.Loaders;
using OpenTK;

namespace Core.Core.Rendering
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

        public TextureMap(string texPath)
        {
            TextureBinding = TextureLoader.LoadTexture(texPath);
            TexturePath = texPath;
        }

        public int RegenerateTexture(string path = null)
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
