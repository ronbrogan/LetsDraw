using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetsDrawATriangle.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexFormat
    {
        Vector3 position;
        Vector2 texture;

        public VertexFormat(Vector3 pos, Vector2 tex)
        {
            position = pos;
            texture = tex;
        }
    }
}
