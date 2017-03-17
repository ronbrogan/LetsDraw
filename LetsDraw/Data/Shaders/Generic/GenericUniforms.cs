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
    public struct GenericUniform : IEquatable<GenericUniform>
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

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ModelMatrix.GetHashCode();
                hashCode = (hashCode*397) ^ NormalMatrix.GetHashCode();
                hashCode = (hashCode*397) ^ DiffuseColor.GetHashCode();
                hashCode = (hashCode*397) ^ SpecularColor.GetHashCode();
                hashCode = (hashCode*397) ^ Alpha.GetHashCode();
                hashCode = (hashCode*397) ^ SpecularExponent.GetHashCode();
                hashCode = (hashCode*397) ^ UseDiffuseMap;
                hashCode = (hashCode*397) ^ UseNormalMap;
                hashCode = (hashCode*397) ^ UseSpecularMap;
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GenericUniform && Equals((GenericUniform)obj);
        }

        public bool Equals(GenericUniform other)
        {
            return ModelMatrix == other.ModelMatrix &&
                NormalMatrix == other.NormalMatrix &&
                DiffuseColor == other.DiffuseColor &&
                SpecularColor == other.SpecularColor &&
                Alpha == other.Alpha &&
                SpecularExponent == other.SpecularExponent &&
                UseDiffuseMap == other.UseDiffuseMap &&
                UseNormalMap == other.UseNormalMap &&
                UseSpecularMap == other.UseSpecularMap;
        }

        public static bool operator ==(GenericUniform g1, GenericUniform g2)
        {
            return g1.Equals(g2);
        }

        public static bool operator !=(GenericUniform g1, GenericUniform g2)
        {
            return !g1.Equals(g2);
        }
    }
}
