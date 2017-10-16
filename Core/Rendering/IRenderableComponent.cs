using System;
using System.Collections.Generic;
using Core.Primitives;

namespace Core.Rendering
{
    public interface IRenderableComponent
    {
        Guid Id { get; }

        WorldTransform Transform { get; }

        List<Mesh> Meshes { get; set; }

    }
}
