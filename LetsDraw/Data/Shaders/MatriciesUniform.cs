using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetsDraw.Data.Shaders
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

        public static readonly int Size = BlittableValueType<MatriciesUniform>.Stride;

    }
}
