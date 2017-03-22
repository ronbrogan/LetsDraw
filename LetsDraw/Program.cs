using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Core;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var engine = new Engine();

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            engine.Start();
        }
    }
}
