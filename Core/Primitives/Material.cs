using Core.Rendering.Enums;
using System;
using System.Numerics;

namespace Core.Primitives
{
    public class Material : IDisposable
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                AmbientMap = null;
                DiffuseMap = null;
                SpecularMap = null;
                SpecularHighlightMap = null;
                AlphaMap = null;
                BumpMap = null;

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion


    }
}
