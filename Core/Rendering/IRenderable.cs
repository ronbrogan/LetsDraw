using System;
using System.Numerics;

namespace Core.Rendering
{
    public interface IRenderable : IDisposable
    {
        void Draw(Matrix4x4 ProjectionMatrix, Matrix4x4 ViewMatrix);
        void Update(double deltaTime = 0);
        
        void Destroy();
    }
}
