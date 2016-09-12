using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LetsDraw.Rendering
{
    public interface IGameObject : IDisposable
    {
        void Draw(Matrix4 ProjectionMatrix, Matrix4 ViewMatrix);
        void Update(double deltaTime = 0);
        void SetShader(int ProgramHandle);

        void SetTexture(string textureName, uint handle);
        
        void Destroy();



        uint GetVao();

        List<uint> GetVbos();
    }
}
