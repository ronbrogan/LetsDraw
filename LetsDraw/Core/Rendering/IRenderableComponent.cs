using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Core.Rendering
{
    public interface IRenderableComponent
    {
        List<RenderMesh> Meshes { get; set; }

        void Draw();

    }
}
