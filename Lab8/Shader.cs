using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8
{
    internal class Shader
    {
        public int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexSource = File.ReadAllText(vertexPath);
            string fragmentSource = File.ReadAllText(fragmentPath);
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vStatus);

            if (vStatus == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                throw new Exception("Ошибка компиляции вершинного шейдера:\n" + infoLog);
            }

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fStatus);

            if (fStatus == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                throw new Exception("Ошибка компиляции фрагментного шейдера:\n" + infoLog);
            }
        }

        public void Use() => GL.UseProgram(Handle);

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetVector3(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform3(location, value);
        }

        public void SetFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value);
        }
        public void SetBool(string name, bool value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value ? 1 : 0);
        }
    }
}
