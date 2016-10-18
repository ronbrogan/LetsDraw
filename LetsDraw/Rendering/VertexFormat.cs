using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetsDraw.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexFormat
    {
        Vector3 position;
        Vector2 texture;
        Vector3 normal;

        public VertexFormat(Vector3 pos, Vector2 tex, Vector3 norm)
        {
            position = pos;
            texture = tex;
            normal = norm;
        }
    }
}
