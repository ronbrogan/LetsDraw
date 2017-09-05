using Foundation.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Core.Primitives
{
    public class Cuboid
    {
        // Assign max to lowers and min to uppers
        // Allows easier min/max operations 
        public float LowerX = float.MaxValue;
        public float UpperX = float.MinValue;
        public float LowerY = float.MaxValue;
        public float UpperY = float.MinValue;
        public float LowerZ = float.MaxValue;
        public float UpperZ = float.MinValue;

        public List<Vector3> GetVerticies()
        {
            var verts = new List<Vector3>();

            verts.Add(new Vector3(LowerX, LowerY, LowerZ));
            verts.Add(new Vector3(UpperX, LowerY, LowerZ));
            verts.Add(new Vector3(UpperX, UpperY, LowerZ));
            verts.Add(new Vector3(LowerX, UpperY, LowerZ));


            verts.Add(new Vector3(LowerX, LowerY, UpperZ));
            verts.Add(new Vector3(UpperX, LowerY, UpperZ));
            verts.Add(new Vector3(UpperX, UpperY, UpperZ));
            verts.Add(new Vector3(LowerX, UpperY, UpperZ));
            return verts;
        }

        public int[] GetIndicies()
        {
            return new[] {
                0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 4, 5, 5, 6, 6, 7, 7, 3, 3, 2, 2, 6, 1, 5, 7, 4
            };
        }

        public float CenterX()
        {
            return SizeX() / 2 + LowerX;
        }

        public float SizeX()
        {
            return UpperX - LowerX;
        }

        public float CenterY()
        {
            return SizeY() / 2 + LowerY;
        }

        public float SizeY()
        {
            return UpperY - LowerY;
        }

        public float CenterZ()
        {
            return SizeZ() / 2 + LowerZ;
        }

        public float SizeZ()
        {
            return UpperZ - LowerZ;
        }

        public Vector3 Center()
        {
            return new Vector3(CenterX(), CenterY(), CenterZ());
        }

        // Sorely lacking in the rotation game
        public static Cuboid Transform(Cuboid input, WorldTransform transform)
        {
            var xformMat = transform.GetTransform();

            var pos = Vector3.Transform(input.Center(), xformMat);
            var newXWidth = input.SizeX() * transform.Scale;
            var newYWidth = input.SizeY() * transform.Scale;
            var newZWidth = input.SizeZ() * transform.Scale;

            return new Cuboid()
            {
                LowerX = pos.X - (newXWidth / 2),
                UpperX = pos.X + (newXWidth / 2),
                LowerY = pos.Y - (newYWidth / 2),
                UpperY = pos.Y + (newYWidth / 2),
                LowerZ = pos.Z - (newZWidth / 2),
                UpperZ = pos.Z + (newZWidth / 2),
            };
        }
    }
}
