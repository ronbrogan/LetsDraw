using System;
using System.Numerics;
using Foundation.Core.Rendering;
using Foundation.Managers;
using OpenTK;
using OpenTK.Input;
using Quat = System.Numerics.Quaternion;
using Vec3 = System.Numerics.Vector3;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace Foundation.World.Cameras
{
    public class FpCamera : ICamera
    {
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public Vector3 Position { get; set; }
        public Matrix4 ViewMatrix { get; set; }
        public Matrix4 ProjectionMatrix { get; set; }

        private float piOverTwo = (float)Math.PI / 2;
        private float speed = 10f;
        private float fov = (float)Math.PI / 2;

        public bool flyMode = false;

        private Vector2 MousePosition;
        private bool isMousePressed = false;


        public FpCamera(Vector3 startingPosition)
        {
            Pitch = 1;
            Yaw = 0;
            Position = startingPosition;
            UpdateView();
        }

        public void UpdateCamera(double deltaTime)
        {
            foreach (var key in InputManager.DownKeys)
                KeyPressed(key, deltaTime);

            if(isMousePressed == false && InputManager.MouseDown == true)
            {
                MousePosition.X = InputManager.MousePosition.X;
                MousePosition.Y = InputManager.MousePosition.Y;
            }

            isMousePressed = InputManager.MouseDown;

            MouseMove((int)InputManager.MousePosition.X, (int)InputManager.MousePosition.Y);

            UpdateView();
        }

        private void UpdateView()
        {
            // Clamp Pitch to +- 90deg
            Pitch = Math.Max(Math.Min(piOverTwo, Pitch), piOverTwo * -1);

            var qPitch = Quat.CreateFromAxisAngle(new Vec3(1, 0, 0), Pitch);
            var qYaw = Quat.CreateFromAxisAngle(new Vec3(0, 1, 0), Yaw);

            //For a FPS camera we can omit roll
            var orient = Quat.Normalize(qPitch * qYaw);
            var rotation = Matrix4x4.CreateFromQuaternion(orient);
            var translation = Matrix4x4.CreateTranslation(Position.ToNumerics() * -1);

            ViewMatrix = Matrix4x4.Multiply(translation, rotation).ToGl();
        }

        public Matrix4 GetViewMatrix()
        {
            return ViewMatrix;
        }

        public Matrix4 GetProjectionMatrix()
        {
            return ProjectionMatrix;
        }

        public void KeyPressed(Key key, double deltaTime)
        {
            float dx = 0;
            float dz = 0;
            float dy = 0;

            // TODO make this configurable on the fly
            const float sensitivity = 6;

            switch (key)
            {
                case Key.W:
                    {
                        dz = sensitivity;
                        break;
                    }

                case Key.S:
                    {
                        dz = -sensitivity;
                        break;
                    }
                case Key.A:
                    {
                        dx = -sensitivity;
                        break;
                    }

                case Key.D:
                    {
                        dx = sensitivity;
                        break;
                    }
                case Key.Space:
                    {
                        dy = sensitivity;
                        break;
                    }
                case Key.LShift:
                    {
                        dy = -sensitivity;
                        break;
                    }
                default:
                    break;
            }

            var mat = GetViewMatrix().ToNumerics();
            
            var iPitch = Quat.CreateFromAxisAngle(new Vec3(1, 0, 0), -Pitch);
            var rotate = Matrix4x4.CreateFromQuaternion(iPitch);

            mat = Matrix4x4.Multiply(mat, rotate);

            var forward = new Vector3(mat.M13, mat.M23, mat.M33);
            var jump = new Vector3(mat.M12, mat.M22, mat.M32);
            var strafe = new Vector3(mat.M11, mat.M21, mat.M31);

            if (!flyMode)
            {
                forward.Y = 0;
            }

            var offset = (-dz * forward + dx * strafe + dy * jump);

            var scaleFactor = (float)deltaTime * speed;

            Position += offset * scaleFactor;
        }

        private void MouseMove(int x, int y)
        {
            if (isMousePressed == false)
                return;

            var mouse_delta = new Vector2(x, y) - MousePosition;

            // TODO make these configurable on the fly
            const float mouseX_Sensitivity = 0.005f;
            const float mouseY_Sensitivity = 0.005f;

            Yaw += mouseX_Sensitivity * mouse_delta.X;
            Pitch += mouseY_Sensitivity * mouse_delta.Y;

            MousePosition = new Vector2(x, y);
        }

        public void UpdateProjectionMatrix(int width, int height)
        {
            float ar = (width / (float)height);

            // TODO make these configurable on the fly
            var near1 = 0.1f;
            var far1 = 8000.0f;

            ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(fov, ar, near1, far1).ToGl();
        }
    }
}
