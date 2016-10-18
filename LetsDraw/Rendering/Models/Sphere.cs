using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Rendering.Models
{
    public class Sphere : Model
    {
        private float pi = (float)Math.PI;
        int indicesSize;
        DateTime startTime;

        public void Create(float radius, uint rings, uint sectors)
        {
            var vertices = new List<VertexFormat>();

            // Generate a sphere
            List<float> vertexes = new List<float>();
            List<float> texcoords = new List<float>();
            List<ushort> indices = new List<ushort>();

            float RingsRecip   = 1.0f / (float)(rings - 1);
            float SectorsRecip = 1.0f / (float)(sectors - 1);
            int countRings, countSectors;

            // Calculate vertices' position and their respective texture coordinates 
            for (countRings = 0; countRings < rings; countRings++)
            {
                float y = (float)Math.Sin(-pi / 2 + pi * countRings * RingsRecip) * radius;

                for (countSectors = 0; countSectors < sectors; countSectors++)
                {
                    float x = (float)Math.Cos(2 * pi * countSectors * SectorsRecip) * (float)Math.Sin(pi * countRings * RingsRecip);
                    float z = (float)Math.Sin(2 * pi * countSectors * SectorsRecip) * (float)Math.Sin(pi * countRings * RingsRecip);

                    vertices.Add(new VertexFormat(new Vector3(x * radius, y, z * radius), new Vector2(countSectors * SectorsRecip, countRings * RingsRecip), Vector3.One));
                }
            }

            // Calculate indices 
            for (countRings = 0; countRings < rings - 1; countRings++)
            {
                for (countSectors = 0; countSectors < sectors - 1; countSectors++)
                {
                    indices.Add((ushort)((countRings + 0) * sectors + countSectors));  
                    indices.Add((ushort)((countRings + 0) * sectors + (countSectors + 1)));
                    indices.Add((ushort)((countRings + 1) * sectors + (countSectors + 1)));
                    indices.Add((ushort)((countRings + 1) * sectors + countSectors));
                }
            }



            uint vao;
            uint vbo;
            uint ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            

            var vertexFormatSize = BlittableValueType.StrideOf<VertexFormat>(new VertexFormat());

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * vertexFormatSize), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(ushort)), indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexFormatSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, vertexFormatSize, 12);

            base.Vao = vao;
            base.Vbos.Add(vbo);
            base.Vbos.Add(ibo);

            indicesSize = indices.Count;
            startTime = DateTime.Now;

            //var error1 = GL.GetError();
            //if (error1 != ErrorCode.NoError)
            //{
            //    Console.WriteLine("-- Error {0} occured at {1}", error1, "some place sphere start");
            //}
            //else
            //{
            //    Console.WriteLine("No Error - End of Sphere Create");
            //}
        }

        public override void Draw(Matrix4 Projection, Matrix4 View)
        {
            GL.UseProgram(base.ShaderProgram);
            GL.BindVertexArray(base.Vao);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, base.Textures["BaseTexture"]);
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "nebulaTex1"), 0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, base.Textures["SecondTexture"]);
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "nebulaTex2"), 1);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, base.Textures["ThirdTexture"]);
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "nebulaTex3"), 2);

            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, base.Textures["AlphaChanTexture"]);
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "alphaChanTex"), 3);

            GL.ActiveTexture(TextureUnit.Texture4);
            GL.BindTexture(TextureTarget.Texture2D, base.Textures["RampTexture"]);
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "rampTex"), 4);


            var endTime = DateTime.Now;                               // get current time
            var dt = endTime - startTime;                            // calculate total elapsed time since app started
            var dtMS = dt.TotalMilliseconds;
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "Timer"), (float)dtMS);  // tuck it in a uniform and pass it on to the shader

            GL.UniformMatrix4(GL.GetUniformLocation(base.ShaderProgram, "view_matrix"), false, ref View);
            GL.UniformMatrix4(GL.GetUniformLocation(base.ShaderProgram, "projection_matrix"), false, ref Projection);

            // Need to draw the object twice since the textures are scrolling and 
            // We do not wish to see overlapping geometry (due to the blend equation)
            // At this point make sure GL_BLEND, GL_CULL_FACE, and GL_DEPTH_TEST are enabled inside the SceneManager.

            GL.CullFace(CullFaceMode.Back); // draw back face 
            GL.DrawElements(PrimitiveType.Quads, indicesSize, DrawElementsType.UnsignedShort, 0);

            GL.CullFace(CullFaceMode.Front); // draw back face 
            GL.DrawElements(PrimitiveType.Quads, indicesSize, DrawElementsType.UnsignedShort, 0);

            //var error1 = GL.GetError();
            //if (error1 != ErrorCode.NoError)
            //{
            //    Console.WriteLine("-- Error {0} occured at {1}", error1, "some place sphere end");
            //}
            //else
            //{
            //    //Console.WriteLine("No Error - End of Sphere Draw");
            //}
        }

    }
}
