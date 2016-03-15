using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using LetsDrawATriangle.Core;

namespace LetsDrawATriangle.Rendering
{
    public class FpCamera
    {
        private float speed = 0.01f;
        private float fov = (float)Math.PI / 3;



        private Vector2 MousePosition;
        private bool isMousePressed = false;

        private float Pitch = 0f;
        private float Yaw = 0f;
        private Vector3 EyeVector;

        private Matrix4 ViewMatrix;
        private Matrix4 ProjectionMatrix;

        public FpCamera(Vector3 startingPosition)
        {
            EyeVector = startingPosition;
            UpdateView();
        }

        public void UpdateView()
        {
            Quaternion qPitch = Quaternion.FromAxisAngle(new Vector3(1, 0, 0), Pitch);
            Quaternion qYaw = Quaternion.FromAxisAngle(new Vector3(0, 1, 0), Yaw);

            //For a FPS camera we can omit roll
            Quaternion orientation = qPitch * qYaw;
            orientation = Quaternion.Normalize(orientation);
            Matrix4 rotate = Matrix4.CreateFromQuaternion(orientation);

            Matrix4 translate = Matrix4.Identity;

            var look = EyeVector * -1;
            Matrix4.CreateTranslation(ref look, out translate);

            Matrix4.Mult(ref translate, ref rotate, out ViewMatrix);
        }

        public Matrix4 GetViewMatrix()
        {
            return ViewMatrix;
        }

        public Matrix4 GetProjectionMatrix()
        {
            return ProjectionMatrix;
        }

        public void KeyPressed(Key key)
        {
            float dx = 0;
            float dz = 0;

            float sensitivity = 0.05f;

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
                default:
                    break;
            }

            var mat = GetViewMatrix();

            var forward = new Vector3(mat[0, 2], mat[1, 2], mat[2, 2]);
            var strafe = new Vector3(mat[0,0], mat[1,0], mat[2,0]);

            EyeVector += (-dz * forward + dx * strafe) * speed;

            UpdateView();
        }

        public void MouseMove(int x, int y)
        {
            if (isMousePressed == false)
                return;

            //always compute delta
            //mousePosition is the last mouse position
            var mouse_delta = new Vector2(x, y) - MousePosition;

            const float mouseX_Sensitivity = 0.01f;
            const float mouseY_Sensitivity = 0.01f;
            //note that yaw and pitch must be converted to radians.
            //this is done in UpdateView() by glm::rotate
            Yaw += mouseX_Sensitivity * mouse_delta.X;
            Pitch += mouseY_Sensitivity * mouse_delta.Y;

            MousePosition = new Vector2(x, y);
            UpdateView();
        }

        public void MousePressed(int x, int y)
        {
            isMousePressed = true;
            MousePosition.X = x;
            MousePosition.Y = y;
        }

        public void MouseReleased()
        {
            isMousePressed = false;
        }

        public void UpdateProjectionMatrix(int width, int height)
        {
            float ar = (width / (float)height);
            var near1 = 0.1f;
            var far1 = 2000.0f;

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, ar, near1, far1);
        }
    }
}
