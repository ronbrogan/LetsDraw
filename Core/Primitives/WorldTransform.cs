using System;
using System.Numerics;

namespace Core.Primitives
{
    public class WorldTransform
    {
        public Guid ParentId { get; set; }
        public Vector3 Position = new Vector3();
        public Vector3 Rotation = new Vector3();

        public float Scale{get;set;}

        private Vector3 lastPosition = new Vector3();
        private Vector3 lastRotation = new Vector3();
        private float lastScale = 1f;
        private Matrix4x4 lastTransform = Matrix4x4.Identity;

        public WorldTransform(Guid parent)
        {
            ParentId = parent;
            Scale = 1f;
        }


        public Matrix4x4 GetTransformationMatrix(bool rotateAroundWorldOrigin = false)
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

        public float PositionX
        {
            get { return Position.X; }
            set { Position = new Vector3(value, Position.Y, Position.Z); }
        }

        public float PositionY
        {
            get { return Position.Y; }
            set { Position = new Vector3(Position.X, value, Position.Z); }
        }

        public float PositionZ
        {
            get { return Position.Z; }
            set { Position = new Vector3(Position.X, Position.Y, value); }
        }

        public float RotationX
        {
            get { return Rotation.X; }
            set { Rotation = new Vector3(value, Rotation.Y, Rotation.Z); }
        }

        public float RotationY
        {
            get { return Rotation.Y; }
            set { Rotation = new Vector3(Rotation.X, value, Rotation.Z); }
        }

        public float RotationZ
        {
            get { return Rotation.Z; }
            set { Rotation = new Vector3(Rotation.X, Rotation.Y, value); }
        }

    }
}
