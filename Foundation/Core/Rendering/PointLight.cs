using System.Runtime.InteropServices;
using OpenTK;

namespace Foundation.Core.Rendering
{
    /// <summary>
    /// Each point light must consume 16n bytes of memory for array indexing to be correct,
    /// padding is added as necessary to ensure that is adhered to. Also, bools are marshaled 
    /// strangely, so using floats internally.
    /// </summary>

    [StructLayout(LayoutKind.Explicit)]
    public struct PointLight
    {
        [FieldOffset(0)]
        public Vector4 Position;

        [FieldOffset(16)]
        public Vector4 Color;

        [FieldOffset(32)]
        public float Intensity;

        [FieldOffset(36)]
        public float Range;

        [FieldOffset(40)]
        private float castsShadows;

        [FieldOffset(44)]
        private float anotherFlag;

        public void CastsShadows(bool cast)
        {
            castsShadows = cast ? 1f : 0f;
        }

        public bool CastShadows()
        {
            return castsShadows == 1f;
        }

        public void AnotherFlag(bool flag)
        {
            anotherFlag = flag ? 1f : 0f;
        }

        public bool AnotherFlag()
        {
            return anotherFlag == 1f;
        }

        public static readonly int Size = BlittableValueType<PointLight>.Stride;
    }
}
