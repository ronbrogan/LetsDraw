using OpenTK;

namespace Foundation.Core.Rendering
{
    public interface ICamera
    {
        float Pitch { get; set; }
        float Yaw { get; set; }
        Vector3 Position { get; set; }
        Matrix4 ViewMatrix { get; set; }
        Matrix4 ProjectionMatrix { get; set; }

        void UpdateCamera(double deltaTime);
        void UpdateProjectionMatrix(int width, int height);
        Matrix4 GetViewMatrix();
        Matrix4 GetProjectionMatrix();
    }
}
