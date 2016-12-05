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
    public struct GenericUniform
    {
        [FieldOffset(0)]
        public Matrix4 ModelMatrix;

        [FieldOffset(64)]
        public Matrix4 NormalMatrix;

        [FieldOffset(128)]
        public Vector4 DiffuseColor;

        [FieldOffset(144)]
        public Vector4 SpecularColor;

        [FieldOffset(160)]
        public float Alpha;

        [FieldOffset(164)]
        public float SpecularExponent;

        [FieldOffset(168)]
        public int UseDiffuseMap;

        [FieldOffset(172)]
        public int UseNormalMap;

        [FieldOffset(176)]
        public int UseSpecularMap;

        public static readonly int Size = BlittableValueType<GenericUniform>.Stride;
    }
}
