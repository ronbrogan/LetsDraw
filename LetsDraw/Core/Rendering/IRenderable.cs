using System;
using System.Collections.Generic;
using OpenTK;

namespace LetsDraw.Core.Rendering
{
    public interface IRenderable : IDisposable
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
