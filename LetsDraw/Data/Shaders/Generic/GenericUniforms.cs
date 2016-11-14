using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetsDraw.Data.Shaders.Generic
{
    [StructLayout(LayoutKind.Explicit)]
    struct GenericUniform
    {
        [FieldOffset(0)]
        public Matrix4 ModelMatrix;

        [FieldOffset(64)]
        public Matrix4 ViewMatrix;

        [FieldOffset(128)]
        public Matrix4 ProjectionMatrix;

        [FieldOffset(192)]
        public Matrix4 NormalMatrix;

        [FieldOffset(256)]
        public Vector3 DiffuseColor;

        [FieldOffset(268)]
        public float Alpha;

        [FieldOffset(272)]
        public int UseDiffuseMap;

        public static readonly int Size = BlittableValueType<GenericUniform>.Stride;
    }
}
