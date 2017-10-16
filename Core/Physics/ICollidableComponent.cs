using Core.Primitives;
using System;
using System.Collections.Generic;

namespace Core.Physics
{
    public interface ICollidableComponent
    {
        Guid Id { get; }

        List<Mesh> CollisionMeshes { get; set; }

        WorldTransform Transform { get; }
    }
}
