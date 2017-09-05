using Foundation.Core.Primitives;
using Foundation.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Core.Physics
{
    public interface ICollidableComponent
    {
        Guid Id { get; }

        List<Mesh> CollisionMeshes { get; set; }

        WorldTransform Transform { get; }
    }
}
