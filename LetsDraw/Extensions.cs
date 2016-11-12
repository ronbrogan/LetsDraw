using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Numerics;
using Quaternion = OpenTK.Quaternion;

namespace LetsDraw
{
    public static class Extensions
    {
        public static string ReduceWhitespace(this string value)
        {
            var newString = new StringBuilder();
            var previousIsWhitespace = false;
            foreach (var c in value)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (previousIsWhitespace)
                    {
                        continue;
                    }

                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }

                newString.Append(c);
            }

            return newString.ToString();
        }

        public static Matrix4x4 ToNumerics(this Matrix4 mat)
        {
            return new Matrix4x4(mat.M11, mat.M12, mat.M13, mat.M14, mat.M21, mat.M22, mat.M23, mat.M24, mat.M31, mat.M32, mat.M33, mat.M34, mat.M41, mat.M42, mat.M43, mat.M44);
        }

        public static Matrix4 ToGl(this Matrix4x4 mat)
        {
            return new Matrix4(mat.M11, mat.M12, mat.M13, mat.M14, mat.M21, mat.M22, mat.M23, mat.M24, mat.M31, mat.M32, mat.M33, mat.M34, mat.M41, mat.M42, mat.M43, mat.M44);
        }

        public static float[] ToArray(this Matrix4 mat)
        {
            return new [] { mat.M11, mat.M12, mat.M13, mat.M14, mat.M21, mat.M22, mat.M23, mat.M24, mat.M31, mat.M32, mat.M33, mat.M34, mat.M41, mat.M42, mat.M43, mat.M44 };
        }

        public static Matrix4 MatrixFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            return Matrix4.CreateFromQuaternion(QuaternionFromYawPitchRoll(yaw, pitch, roll));
        }

        public static Quaternion QuaternionFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion result = Quaternion.Identity;
            float num9 = roll * 0.5f;
            float num6 = (float)Math.Sin((double)num9);
            float num5 = (float)Math.Cos((double)num9);
            float num8 = pitch * 0.5f;
            float num4 = (float)Math.Sin((double)num8);
            float num3 = (float)Math.Cos((double)num8);
            float num7 = yaw * 0.5f;
            float num2 = (float)Math.Sin((double)num7);
            float num = (float)Math.Cos((double)num7);
            result.X = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.Y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.Z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.W = ((num * num3) * num5) + ((num2 * num4) * num6);
            return result;
        }
    }
}
