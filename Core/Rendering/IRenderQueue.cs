using Core.Primitives;
using System;
using System.Collections.Generic;

namespace Core.Rendering
{
    public interface IRenderQueue
    {
        Dictionary<int, List<Mesh>> MeshRegistry { get; set; }

        WorldTransform GetTransform(Mesh mesh);

        void Add(Mesh mesh, WorldTransform MeshTransform = null);

        void Add(IRenderableComponent component);

        void UpdateTransform(IRenderableComponent component);

        void Remove(Mesh mesh, bool SkipParentCheck = false);

        void Render();
    }
}
