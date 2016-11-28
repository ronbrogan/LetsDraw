using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core.Rendering;
using LetsDraw.Data.Enums;
using LetsDraw.Loaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using LetsDraw.Rendering;

namespace LetsDraw.Managers
{
    public class ShaderManager
    {
        public static int CurrentShader = -1;
        private static Dictionary<string, int> Shaders = new Dictionary<string, int>();

        public static Dictionary<int, ShaderUniformCatalog> UniformCatalog = new Dictionary<int, ShaderUniformCatalog>();

        public ShaderManager()
        {
            var generic = CreateShader("Generic", "Data/Shaders/Generic/vertexShader.glsl", "Data/Shaders/Generic/fragmentShader.glsl");

            var genericCat = new ShaderUniformCatalog
            {
                NormalMatrix = GL.GetUniformLocation(generic, "normal_matrix"),
                ModelMatrix = GL.GetUniformLocation(generic, "model_matrix"),
                ViewMatrix = GL.GetUniformLocation(generic, "view_matrix"),
                ProjectionMatrix = GL.GetUniformLocation(generic, "projection_matrix"),

                UseDiffuseMap = GL.GetUniformLocation(generic, "use_diffuse_map"),
                DiffuseColor = GL.GetUniformLocation(generic, "diffuse_color"),
                DiffuseMap = GL.GetUniformLocation(generic, "diffuse_map"),
                Alpha = GL.GetUniformLocation(generic, "alpha")
            };

            ShaderManager.UniformCatalog.Add(generic, genericCat);


            CreateShader("HudShader", "Data/Shaders/HUD/hudVertex.glsl", "Data/Shaders/HUD/hudFragment.glsl");
        }

        public static int GetShaderForMaterial(Material mat)
        {
            switch (mat.IlluminationModel)
            {

                default:
                    return Shaders["Generic"];
            }

        }

        private static string ReadShader(string file)
        {
            if(File.Exists(file))
                return File.ReadAllText(file);

            return string.Empty;
        }

        private static int CompileShader(ShaderType type, string sourceCode, string shaderName) 
        {
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

            return shader;
        }

        public static int CreateShader(string shaderName, string vertexFilename, string fragmentFilename, string geometryFilename = null)
        {
            var vertexSource = ReadShader(vertexFilename);
            var fragmentSource = ReadShader(fragmentFilename);

            int vertexShader = 0;
            int fragmentShader = 0;
            int geometryShader = 0;

            if (vertexSource != string.Empty)
                vertexShader = CompileShader(ShaderType.VertexShader, vertexSource, "vertex::" + shaderName);

            if (fragmentSource != string.Empty)
                fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentSource, "fragment::" + shaderName);

            if(!string.IsNullOrWhiteSpace(geometryFilename))
            {
                var geoSource = ReadShader(geometryFilename);

                if (geoSource != string.Empty)
                    geometryShader = CompileShader(ShaderType.GeometryShader, geoSource, "geometry::" + shaderName);
            }

            int linkResult = 0;

            var program = GL.CreateProgram();

            if(vertexShader != 0)
                GL.AttachShader(program, vertexShader);

            if (fragmentShader != 0)
                GL.AttachShader(program, fragmentShader);

            if (geometryShader != 0)
                GL.AttachShader(program, geometryShader);

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

        public static bool SetShader(int ShaderProgram)
        {
            if(CurrentShader != ShaderProgram)
            {
                GL.UseProgram(ShaderProgram);
                CurrentShader = ShaderProgram;
                return true;
            }
            return false;
        }

        public static void Dispose()
        {
            foreach (var program in Shaders)
            {
                GL.DeleteProgram(program.Value);
            }
        }
    }
}
