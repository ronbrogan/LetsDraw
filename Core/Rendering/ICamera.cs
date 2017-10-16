using System.Numerics;

namespace Core.Rendering
{
    public interface ICamera
    {
        float Pitch { get; set; }
        float Yaw { get; set; }
        Vector3 Position { get; set; }
        Matrix4x4 ViewMatrix { get; set; }
        Matrix4x4 ProjectionMatrix { get; set; }

        void UpdateCamera(double deltaTime);
        void UpdateProjectionMatrix(int width, int height);
        Matrix4x4 GetViewMatrix();
        Matrix4x4 GetProjectionMatrix();
    }
}
