using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Core.Rendering
{
    public interface IRenderableComponent : IRenderable
    {
        Matrix4x4 Transform { get; set; }

        List<Mesh> Meshes { get; set; }

    }
}
