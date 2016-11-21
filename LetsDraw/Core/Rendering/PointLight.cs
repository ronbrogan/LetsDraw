using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetsDraw.Core.Rendering
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PointLight
    {
        [FieldOffset(0)]
        public Vector3 Position;

        [FieldOffset(12)]
        public Vector3 Color;

        [FieldOffset(24)]
        public float Intensity;
    }
}
