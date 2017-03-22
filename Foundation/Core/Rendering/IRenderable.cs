using System;
using OpenTK;

namespace Foundation.Core.Rendering
{
    public interface IRenderable : IDisposable
    {
        void Draw(Matrix4 ProjectionMatrix, Matrix4 ViewMatrix);
        void Update(double deltaTime = 0);
        
        void Destroy();
    }
}
