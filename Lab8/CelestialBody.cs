using OpenTK.Mathematics;

namespace Lab8
{
    internal class CelestialBody
    {

        private float _orbitAngle = 0f;
        private float _spinAngle = 0f;
        private readonly SphereMesh _mesh;

        public float DrawRadius;
        public Vector3 Color;
        public float OrbitRadius;
        public float OrbitSpeed;
        public float SpinSpeed;
        public bool IsSun { get; set; } = false;
        public List<CelestialBody> Children { get; } = new();      
        public Vector3 Position { get; private set; }

        public CelestialBody(SphereMesh mesh) => _mesh = mesh;
        
        public void AddChild(CelestialBody child) => Children.Add(child);
        
        public void Update(float deltaTime)
        {
            _orbitAngle += OrbitSpeed * deltaTime;
            _spinAngle += SpinSpeed * deltaTime;

            foreach (var child in Children)
                child.Update(deltaTime);
        }

        public void Render(Shader shader, Matrix4 parentModel)
        {
            var orbitOffset = new Vector3(
                OrbitRadius * MathF.Cos(_orbitAngle),
                0,
                OrbitRadius * MathF.Sin(_orbitAngle));

            var localModel =
                  Matrix4.CreateRotationY(_spinAngle)
                * Matrix4.CreateScale(DrawRadius)
                * Matrix4.CreateTranslation(orbitOffset);

            var model = localModel * parentModel;

            Position = new Vector3(model.M41, model.M42, model.M43);

            shader.SetMatrix4("uModel", model);
            shader.SetVector3("objectColor", Color);
            shader.SetBool("isSun", IsSun);

            _mesh.Bind();
            _mesh.Render();

            foreach (var child in Children)
                child.Render(shader, model);
        }
    }
}
