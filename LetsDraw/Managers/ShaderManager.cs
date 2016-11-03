using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using LetsDraw.Rendering;

namespace LetsDraw.Managers
{
    public static class ShaderManager
    {
        private static Dictionary<string, int> Shaders = new Dictionary<string, int>();

        public static Dictionary<int, ShaderUniformCatalog> UniformCatalog = new Dictionary<int, ShaderUniformCatalog>();


        private static string ReadShader(string file)
        {
            if(File.Exists(file))
                return File.ReadAllText(file);

            return string.Empty;
        }

        private static int CompileShader(ShaderType type, string sourceCode, string shaderName) 
        {
            Console.Write("Compiling shader: {0}...", shaderName);

            int statusCode = 0;
            int shader = GL.CreateShader(type);

            GL.ShaderSource(shader, sourceCode);

            GL.CompileShader(shader);

            var shaderStatus = GL.GetShaderInfoLog(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out statusCode);

            if(statusCode == 0)
            {
                Console.WriteLine("-- Shader Error --");
                Console.WriteLine("-- Could not create shader: {0}", shaderName);
                Console.WriteLine(shaderStatus);
                return 0;
            }

            Console.WriteLine("Done.");

            return shader;
        }

        public static int CreateShader(string shaderName, string vertexFilename, string fragmentFilename)
        {
            var vertexSource = ReadShader(vertexFilename);
            var fragmentSource = ReadShader(fragmentFilename);

            int vertexShader = 0;
            int fragmentShader = 0;

            if (vertexSource != string.Empty)
                vertexShader = CompileShader(ShaderType.VertexShader, vertexSource, "vertex::" + shaderName);

            if (fragmentSource != string.Empty)
                fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentSource, "fragment::" + shaderName);

            int linkResult = 0;

            var program = GL.CreateProgram();

            if(vertexShader != 0)
                GL.AttachShader(program, vertexShader);

            if (fragmentShader != 0)
                GL.AttachShader(program, fragmentShader);

            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linkResult);

            if(linkResult == 0)
            {
                string linkLog;

                GL.GetProgramInfoLog(program, out linkLog);


                Console.WriteLine("CREATE PROGRAM FAILED");
                Console.WriteLine(linkLog);
                return 0;
            }

            Shaders.Add(shaderName, program);

            return program;
        }

        public static int GetShader(string shaderName)
        {
            if (!Shaders.ContainsKey(shaderName))
            {
                Console.WriteLine("-- Shader Error --");
                Console.WriteLine("-- Could not find shader: {0}", shaderName);
                return 0;
            }
                
            return Shaders[shaderName];
        }

        public static void DeleteShader(string shaderName)
        {
            if (!Shaders.ContainsKey(shaderName))
                return;

            GL.DeleteProgram(Shaders[shaderName]);
        }

        public static void Dispose()
        {
            foreach(var program in Shaders)
            {
                GL.DeleteProgram(program.Value);
            }
        }
    }
}
