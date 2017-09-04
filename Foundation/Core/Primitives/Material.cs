using Foundation.Data.Enums;
using OpenTK;

namespace Foundation.Core.Primitives
{
    public class Material
    {
        public string MaterialName { get; set; }

        public Vector3 AmbientColor { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public Vector3 SpecularColor { get; set; }
        public Vector3 TransmissionFilter { get; set; }

        public float SpecularExponent { get; set; }
        public float IndexOfRefraction { get; set; }

        public float Transparency { get; set; }
        public IlluminationModel IlluminationModel { get; set; }

        public TextureMap AmbientMap { get; set; }
        public TextureMap DiffuseMap { get; set; }
        public TextureMap SpecularMap { get; set; }
        public TextureMap SpecularHighlightMap { get; set; }
        public TextureMap AlphaMap { get; set; }
        public TextureMap BumpMap { get; set; }

        public Material(string name)
        {
            MaterialName = name;
        }


    }
}
