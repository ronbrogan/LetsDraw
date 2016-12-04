using System.Runtime.InteropServices;
using OpenTK;

namespace LetsDraw.Loaders
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexFormat
    {
        public Vector3 position;
        public Vector2 texture;
        public Vector3 normal;
        public Vector3 tangent;
        public Vector3 bitangent;

        public VertexFormat(Vector3 pos, Vector2 tex, Vector3 norm)
        {
            position = pos;
            texture = tex;
            normal = norm;
            tangent = Vector3.One;
            bitangent = Vector3.One;
        }

        public VertexFormat(Vector3 pos, Vector2 tex, Vector3 norm, Vector3 tan, Vector3 bitan)
        {
            position = pos;
            texture = tex;
            normal = norm;
            tangent = tan;
            bitangent = bitan;
        }

        public static readonly int Size = BlittableValueType<VertexFormat>.Stride;
    }
}
