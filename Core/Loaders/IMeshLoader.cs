using Core.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Loaders
{
    public interface IMeshLoader
    {
        Guid Id { get; }
        Dictionary <string, Mesh> Meshes { get; set; }
    }
}
