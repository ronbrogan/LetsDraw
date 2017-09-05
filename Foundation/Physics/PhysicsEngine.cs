using Foundation.Core;
using Foundation.Core.Physics;
using Foundation.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Physics
{
    public class PhysicsEngine
    {
        private Dictionary<Guid, ICollidableComponent> RegisteredCollidables { get; set; }

        private List<Tuple<AxisColliderHelper, AxisColliderHelper>> BroadphaseResults { get; set; }
        
        public PhysicsEngine()
        {
            RegisteredCollidables = new Dictionary<Guid, ICollidableComponent>();
        }

        public void RegisterCollidable(ICollidableComponent component)
        {
            if (component == null)
                return;

            RegisteredCollidables[component.Id] = component;
        }

        public void DoBroadPhase()
        {
            var collidables = new List<AxisColliderHelper>();
            foreach (var collidable in RegisteredCollidables)
            {
                foreach(var mesh in collidable.Value.CollisionMeshes)
                {
                    mesh.Colliding = false;
                }

                collidables.Add(AxisColliderHelper.FromICollidable(collidable.Value));
            }

            var xCollisions = GetAxisCollisions(collidables, Axis.X);
            var allXItems = xCollisions.SelectMany(x => new[] { x.Item1, x.Item2 }).Distinct();

            var xyCollisions = GetAxisCollisions(allXItems, Axis.Y);
            var allXyItems = xyCollisions.SelectMany(x => new[] { x.Item1, x.Item2 }).Distinct();

            BroadphaseResults = GetAxisCollisions(allXyItems, Axis.Z);

            var allCollidedMeshIds = BroadphaseResults.SelectMany(x => new[] { x.Item1.Id, x.Item2.Id }).Distinct();

            foreach (var result in BroadphaseResults)
            {
                var components = RegisteredCollidables.Values.Where(c => allCollidedMeshIds.Contains(c.Id));

                foreach(var mesh in components.SelectMany(c => c.CollisionMeshes))
                {
                    mesh.Colliding = true;
                }
            }
        }

        private List<Tuple<AxisColliderHelper, AxisColliderHelper>> GetAxisCollisions(IEnumerable<AxisColliderHelper> input, Axis axis)
        {
            var workingColliders = new List<AxisColliderHelper>();
            var axisCollisions = new List<Tuple<AxisColliderHelper, AxisColliderHelper>>();
            var invalidColliders = new List<Guid>();

            if (!input.Any())
                return axisCollisions;

            var items = input.OrderBy(o => o.Min.Axis(axis));

            workingColliders.Add(items.First());

            // Starting at 1, because we already added the first element
            for (var i = 1; i < items.Count(); i++)
            {
                var currentItem = items.ElementAt(i);

                for (var c = 0; c < workingColliders.Count; c++)
                {
                    var candidate = workingColliders.ElementAt(c);

                    if (currentItem.Min.Axis(axis) <= candidate.Max.Axis(axis))
                    {
                        axisCollisions.Add(new Tuple<AxisColliderHelper, AxisColliderHelper>(currentItem, candidate));
                    }
                    else
                    {
                        invalidColliders.Add(candidate.Id);
                    }
                }

                workingColliders.Add(currentItem);

                workingColliders.RemoveAll(h => invalidColliders.Contains(h.Id));

                invalidColliders.Clear();
            }

            return axisCollisions;
        }

    }
}
