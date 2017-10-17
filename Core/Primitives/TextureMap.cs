using Core.Loaders;
using Core.Rendering.Enums;
using System;
using System.IO;
using System.Numerics;

namespace Core.Primitives
{
    public class TextureMap : IDisposable
    {
        public uint TextureArray { get; set; }
        public int ArrayIndex { get; set; }

        private int? textureBinding = null;
        public int TextureBinding {
            get
            {
                if(textureBinding.HasValue)
                    return textureBinding.Value;

                throw new Exception("Texture was not bound before usage.");
            }
            private set
            {
                textureBinding = value;
            }
        }

        public Vector3 OriginOffset { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Turbulence { get; set; }

        public float BumpMultiplier { get; set; }

        public ImfChanOption ChannelOption { get; set; }

        private Stream textureData;

        // Empty constructor for serializer compat
        public TextureMap()
        {
            
        }

        public TextureMap(string texPath)
        {
            textureData = new MemoryStream();

            using (var fs = new FileStream(texPath, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(textureData);
            }

            textureData.Seek(0, SeekOrigin.Begin);
        }

        public int Bind(ITextureBinder binder)
        {
            TextureBinding = binder.Bind(textureData);
            return TextureBinding;
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
                    textureData.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TextureMap() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
