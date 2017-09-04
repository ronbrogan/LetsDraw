using System;
using System.Collections.Generic;
using System.Numerics;
using Foundation.World;
using Foundation.Core.Primitives;

namespace Foundation.Core.Rendering
{
    public interface IRenderableComponent
    {
        Guid Id { get; }

        WorldTransform Transform { get; }

        List<Mesh> Meshes { get; set; }

    }
}
