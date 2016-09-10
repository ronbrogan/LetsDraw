using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Rendering.Models
{
    public class Model : IGameObject
    {
        protected uint Vao;
        protected List<uint> Vbos;
        protected int Program;

        protected Dictionary<string, uint> Textures;

        public Model()
        {
            Vbos = new List<uint>();
            Textures = new Dictionary<string, uint>();
        }

        public virtual void Draw(Matrix4 ProjectionMatrix, Matrix4 ViewMatrix)
        {

        }

        public virtual void Update()
        {
            
        }

        public virtual void SetShader(int ProgramHandle)
        {
            Program = ProgramHandle;
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
