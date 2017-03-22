using System;
using System.Collections.Generic;
using System.Numerics;

namespace Foundation.Core.Rendering
{
    public interface IRenderableComponent
    {
        Guid Id { get; }

        Matrix4x4 Transform { get; }

        List<Mesh> Meshes { get; set; }

    }
}
