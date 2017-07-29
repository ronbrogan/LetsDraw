using System;
using System.Numerics;
using Foundation.Core;
using Foundation.Rendering;
using Vector3 = OpenTK.Vector3;

namespace Foundation.World
{
    public class WorldTransform
    {
        public Guid ParentId { get; set; }
        public Vector3 Position = new Vector3();
        public Vector3 Rotation = new Vector3();

        private float _scale;
        public float Scale
        {
            get;

            set;
        }

        private Vector3 lastPosition = new Vector3();
        private Vector3 lastRotation = new Vector3();
        private float lastScale = 1f;
        private Matrix4x4 lastTransform = Matrix4x4.Identity;

        public WorldTransform(Guid parent)
        {
            ParentId = parent;
            Scale = 1f;
        }


        public Matrix4x4 GetTransform(bool rotateAroundWorldOrigin = false)
        {
            if (lastPosition == Position && lastRotation == Rotation && !(Math.Abs(lastScale - Scale) > 0.01))
                return lastTransform;

            lastPosition = Position;
            lastRotation = Rotation;
            lastScale = Scale;

            var position = Matrix4x4.CreateTranslation(Position.X, Position.Y, Position.Z);
            var rotation = Matrix4x4.CreateFromQuaternion(System.Numerics.Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z));
            var scale = Matrix4x4.CreateScale(Scale);

            lastTransform = (rotation * scale * position);
            return lastTransform;
        }

    }
}
