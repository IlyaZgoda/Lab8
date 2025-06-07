using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Lab8
{
    public class Game : GameWindow
    { 
        private Shader _shader;
        private Camera _camera;
        private float _lastX;
        private float _lastY;
        private bool _isFirstMove = true;
        private List<CelestialBody> _bodies = new();
        private SphereMesh _sphereMesh;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.1f, 1.0f); 
            GL.Enable(EnableCap.DepthTest);

            _shader = new Shader("shader.vert", "shader.frag");
            _camera = new(new Vector3(0, 0, 10)); 
            _sphereMesh = new SphereMesh(1.0f);

            LoadSolarSystem();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _shader.SetFloat("time", (float)args.Time);

            var view = _camera.ViewMatrix;
            var projection = Matrix4.CreatePerspectiveFieldOfView(
                                MathHelper.DegreesToRadians(45f),
                                Size.X / (float)Size.Y,
                                0.1f, 100f);

            _shader.SetMatrix4("uView", view);
            _shader.SetMatrix4("uProjection", projection);

            SetLightAndMaterialUniforms();

            foreach (var body in _bodies)
                body.Render(_shader, Matrix4.Identity);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            CursorState = CursorState.Grabbed;

            if (_isFirstMove)
            {
                _lastX = e.X;
                _lastY = e.Y;
                _isFirstMove = false;
            }

            float deltaX = e.X - _lastX;
            float deltaY = _lastY - e.Y;

            _lastX = e.X;
            _lastY = e.Y;

            _camera.Look(deltaX, deltaY);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;

            _camera.Move(input, (float)args.Time);

            if (input.IsKeyDown(Keys.Escape))
                Close();
            foreach (var body in _bodies)
                body.Update((float)args.Time);

        }

        private void SetLightAndMaterialUniforms()
        {
            if (_bodies.Count > 0 && _bodies[0].IsSun)
            {
                _shader.SetVector3("lightPos", _bodies[0].Position);
                _shader.SetVector3("lightColor", new Vector3(1.0f, 0.9f, 0.7f));


            }
            else
            {
                _shader.SetVector3("lightPos", Vector3.Zero);
                _shader.SetVector3("lightColor", Vector3.One);

            }

            _shader.SetVector3("viewPos", _camera.Position);
        }

        private void LoadSolarSystem()
        {
            var sun = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 1.0f,
                Color = new Vector3(1.5f, 1.2f, 0.6f),
                OrbitRadius = 0.0f,
                OrbitSpeed = 0.0f,
                SpinSpeed = 0.02f,
                IsSun = true
            };

            var mercury = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.05f,
                Color = new Vector3(0.7f, 0.6f, 0.5f),
                OrbitRadius = 1.5f,
                OrbitSpeed = 4.1f,
                SpinSpeed = 0.005f
            };

            var venus = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.12f,
                Color = new Vector3(0.9f, 0.7f, 0.4f),
                OrbitRadius = 2.0f,
                OrbitSpeed = 1.6f,
                SpinSpeed = -0.002f
            };

            var earth = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.13f,
                Color = new Vector3(0.2f, 0.5f, 0.9f),
                OrbitRadius = 3.0f,
                OrbitSpeed = 1.0f,
                SpinSpeed = 1.0f
            };

            var mars = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.07f,
                Color = new Vector3(0.8f, 0.3f, 0.1f),
                OrbitRadius = 4.5f,
                OrbitSpeed = 0.5f,
                SpinSpeed = 0.9f
            };

            var jupiter = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.5f,
                Color = new Vector3(0.8f, 0.6f, 0.4f),
                OrbitRadius = 6.0f,
                OrbitSpeed = 0.2f,
                SpinSpeed = 2.4f
            };

            var saturn = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.4f,
                Color = new Vector3(0.9f, 0.8f, 0.5f),
                OrbitRadius = 7.5f,
                OrbitSpeed = 0.15f,
                SpinSpeed = 2.0f
            };

            var uranus = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.3f,
                Color = new Vector3(0.6f, 0.8f, 0.9f),
                OrbitRadius = 9.0f,
                OrbitSpeed = 0.1f,
                SpinSpeed = -1.4f
            };

            var neptune = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.29f,
                Color = new Vector3(0.2f, 0.3f, 0.8f),
                OrbitRadius = 10.5f,
                OrbitSpeed = 0.08f,
                SpinSpeed = 1.5f
            };

            var moon = new CelestialBody(_sphereMesh)
            {
                DrawRadius = 0.03f,
                Color = new Vector3(0.8f, 0.8f, 0.8f),
                OrbitRadius = 1.4f,
                OrbitSpeed = 12.0f,
                SpinSpeed = 0.0f
            };

            earth.AddChild(moon);
            sun.AddChild(mercury);
            sun.AddChild(venus);
            sun.AddChild(earth);
            sun.AddChild(mars);
            sun.AddChild(jupiter);
            sun.AddChild(saturn);
            sun.AddChild(uranus);
            sun.AddChild(neptune);

            _bodies.Add(sun);
        }
    }
}
