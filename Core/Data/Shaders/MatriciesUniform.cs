using System.Runtime.InteropServices;
using OpenTK;

namespace Core.Data.Shaders
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MatriciesUniform
    {
        [FieldOffset(0)]
        public Matrix4 ViewMatrix;

        [FieldOffset(64)]
        public Matrix4 ProjectionMatrix;

        [FieldOffset(128)]
        public Matrix4 DetranslatedViewMatrix;

        [FieldOffset(192)]
        public Vector3 ViewPosition;

        public static readonly int Size = BlittableValueType<MatriciesUniform>.Stride;

    }
}
