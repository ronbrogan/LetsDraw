using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Loaders
{
    public interface ITextureBinder
    {
        int Bind(Stream textureData);
    }
}
