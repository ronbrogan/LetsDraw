using System.Collections.Generic;
using Core.Core.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core.Rendering.Models
{
    public class Model : IRenderable
    {
        protected uint Vao;
        protected List<uint> Vbos;
        protected int ShaderProgram;

        protected Dictionary<string, uint> Textures;

        public Model()
        {
            Vbos = new List<uint>();
            Textures = new Dictionary<string, uint>();
        }

        public virtual void Draw(Matrix4 ProjectionMatrix, Matrix4 ViewMatrix)
        {

        }

        public virtual void Draw() { }

        public virtual void Update(double deltaTime = 0)
        {
            
        }

        public virtual void SetShader(int ProgramHandle)
        {
            ShaderProgram = ProgramHandle;
        }

        public void SetTexture(string textureName, uint glTextureHandle)
        {
            Textures.Add(textureName, glTextureHandle);
        }

        public virtual void Destroy()
        {
            GL.DeleteVertexArrays(1, ref Vao);
            GL.DeleteBuffers(Vbos.Count, Vbos.ToArray());
        }

        public virtual uint GetVao()
        {
            return Vao;
        }

        public virtual List<uint> GetVbos()
        {
            return Vbos;
        }

        public virtual void Dispose()
        {
            Destroy();
        }
    }
}
