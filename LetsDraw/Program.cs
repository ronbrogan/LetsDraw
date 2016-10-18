using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Managers;
using LetsDraw.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw
{
    static class Program
    {
        

        [STAThread]
        static void Main(string[] args)
        {
            var engine = new Engine();

            Console.Clear();
            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            engine.Start();
        }

        

    }
}
