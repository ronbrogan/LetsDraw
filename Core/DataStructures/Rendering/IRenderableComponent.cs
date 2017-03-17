using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Core.Rendering
{
    public interface IRenderableComponent
    {
        Guid Id { get; }

        Matrix4x4 Transform { get; }

        List<Mesh> Meshes { get; set; }

    }
}
