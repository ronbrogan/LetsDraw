using Core.Physics;
using Core.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Foundation.Physics
{
    public class AxisColliderHelper
    {
        public Guid Id { get; set; }

        public Vector3 Min { get; set; }

        public Vector3 Max { get; set; }


        public static AxisColliderHelper FromICollidable(ICollidableComponent component)
        {
            var componentBb = new Cuboid();

            foreach(var mesh in component.CollisionMeshes)
            {
                componentBb.LowerX = Math.Min(componentBb.LowerX, mesh.BoundingBox.LowerX);
                componentBb.UpperX = Math.Max(componentBb.UpperX, mesh.BoundingBox.UpperX);
                componentBb.LowerY = Math.Min(componentBb.LowerY, mesh.BoundingBox.LowerY);
                componentBb.UpperY = Math.Max(componentBb.UpperY, mesh.BoundingBox.UpperY);
                componentBb.LowerZ = Math.Min(componentBb.LowerZ, mesh.BoundingBox.LowerZ);
                componentBb.UpperZ = Math.Max(componentBb.UpperZ, mesh.BoundingBox.UpperZ);
            }

            var bb = Cuboid.Transform(componentBb, component.Transform);

            return new AxisColliderHelper()
            {
                Id = component.Id,
                Min = new Vector3(bb.LowerX, bb.LowerY, bb.LowerZ),
                Max = new Vector3(bb.UpperX, bb.UpperY, bb.UpperZ)
            };
        }

        public override bool Equals(object obj)
        {
            var helper = obj as AxisColliderHelper;
            return helper != null &&
                   Id.Equals(helper.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }
    }
}
