using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDraw.Rendering
{
    public class ShaderUniformCatalog
    {
        public int NormalMatrix { get; set; }

        public int ModelMatrix { get; set; }

        public int ViewMatrix { get; set; }

        public int ProjectionMatrix { get; set; }
    }
}
