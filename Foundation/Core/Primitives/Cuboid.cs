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
    }
}
