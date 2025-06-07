using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lab8
{
    internal class SphereMesh
    {
        private int _vao, _vbo, _ebo;
        private int _indexCount;

        public SphereMesh(float radius, int sectors = 48, int stacks = 48)
        {
            var vertices = new List<float>();
            var indices = new List<uint>();

            for (int i = 0; i <= stacks; ++i)
            {
                float stackAngle = MathF.PI / 2f - i * MathF.PI / stacks;
                float xy = radius * MathF.Cos(stackAngle);
                float z = radius * MathF.Sin(stackAngle);

                for (int j = 0; j <= sectors; ++j)
                {
                    float sectorAngle = j * 2f * MathF.PI / sectors;
                    float x = xy * MathF.Cos(sectorAngle);
                    float y = xy * MathF.Sin(sectorAngle);

                    var pos = new Vector3(x, y, z);
                    var normal = Vector3.Normalize(pos);

                    vertices.AddRange([pos.X, pos.Y, pos.Z, normal.X, normal.Y, normal.Z]);
                }
            }

            for (int i = 0; i < stacks; ++i)
            {
                int k1 = i * (sectors + 1);
                int k2 = k1 + sectors + 1;

                for (int j = 0; j < sectors; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add((uint)k1);
                        indices.Add((uint)k2);
                        indices.Add((uint)(k1 + 1));
                    }
                    if (i != (stacks - 1))
                    {
                        indices.Add((uint)(k1 + 1));
                        indices.Add((uint)k2);
                        indices.Add((uint)(k2 + 1));
                    }
                }
            }

            _indexCount = indices.Count;

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);

            int stride = 6 * sizeof(float);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }

        public void Bind() => GL.BindVertexArray(_vao);
        public void Render() => GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
    }
}
