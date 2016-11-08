using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Core.Rendering
{
    public interface IRenderableComponent : IRenderable
    {
        List<Mesh> Meshes { get; set; }

    }
}
