using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lab8
{
    internal class Camera
    {
        public Vector3 Position;
        public Vector3 Front = -Vector3.UnitZ;
        public Vector3 Up = Vector3.UnitY;

        private float _yaw = -90f;
        private float _pitch = 0f;

        private float _sensitivity = 0.2f;
        private float _cameraSpeed = 1.5f;

        public Matrix4 ViewMatrix => 
            Matrix4.LookAt(Position, Position + Front, Up);

        public Camera(Vector3 position) =>
            Position = position;

        public void Look(float deltaX, float deltaY)
        {
            deltaX *= _sensitivity;
            deltaY *= _sensitivity;

            _yaw += deltaX;
            _pitch += deltaY;

            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);

            Vector3 direction;

            direction.X = MathF.Cos(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch));
            direction.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            direction.Z = MathF.Sin(MathHelper.DegreesToRadians(_yaw)) * MathF.Cos(MathHelper.DegreesToRadians(_pitch));

            Front = Vector3.Normalize(direction);
        }

        public void Move(KeyboardState input, float time)
        {
            float speedMultiplier = input.IsKeyDown(Keys.LeftShift) ? 2.0f : 1.0f;
            float velocity = _cameraSpeed * speedMultiplier * time;

            if (input.IsKeyDown(Keys.W))
                Position += Front * velocity;
            if (input.IsKeyDown(Keys.S))
                Position -= Front * velocity;
            if (input.IsKeyDown(Keys.A))
                Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * velocity;
            if (input.IsKeyDown(Keys.D))
                Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * velocity;
        }
    }
}
