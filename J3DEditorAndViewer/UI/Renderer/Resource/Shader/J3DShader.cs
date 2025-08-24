// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J3DEditorAndViewer.FileFormat.SectionFormat;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal class J3DShader : IShader
    {
        private bool _disposed = false;

        private int Handle;

        private static (int, int) ShaderCompilerVertex(IVertexFactory res)
        {
            int shaderID = GL.CreateShader(ShaderType.VertexShader);

            StringBuilder builder = new();
            int bindingID = res.WriteDefinicator(builder);

            Debug.WriteLine($"Shader Source:\n{builder.ToString()}");
            GL.ShaderSource(shaderID, builder.ToString());
            GL.CompileShader(shaderID);
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shaderID);
                Debug.WriteLine(infoLog);
            }

            return (shaderID, bindingID);
        }
        private static int ShaderCompilerFragment(IFragmentFactory res, int bindingID)
        {
            int shaderID = GL.CreateShader(ShaderType.FragmentShader);

            StringBuilder builder = new();
            res.WriteDefinicator(builder, bindingID);

            Debug.WriteLine($"Shader Source:\n{builder.ToString()}");
            GL.ShaderSource(shaderID, builder.ToString());
            GL.CompileShader(shaderID);
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shaderID);
                Debug.WriteLine(infoLog);
            }

            return shaderID;
        }

        private static int ShaderLinker(int vertex, int fragment)
        {
            int handle = GL.CreateProgram();
            {
                GL.AttachShader(handle, vertex);
                GL.AttachShader(handle, fragment);

                GL.LinkProgram(handle);

                GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
                if (success == 0)
                {
                    string infoLog = GL.GetProgramInfoLog(handle);
                    Debug.WriteLine(infoLog);
                }
            }

            GL.DetachShader(handle, vertex);
            GL.DetachShader(handle, fragment);

            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);

            return handle;
        }

        public J3DShader(IVertexFactory vertexFactory, IFragmentFactory fragmentFactory)
        {
            _disposed = false;

            (int vertexShader, int bindingID) = ShaderCompilerVertex(vertexFactory);
            int fragmentShader = fragmentShader = ShaderCompilerFragment(fragmentFactory, bindingID);

            Handle = ShaderLinker(vertexShader, fragmentShader);
        }

        ~J3DShader()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                GL.DeleteProgram(Handle);
            }
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }
    }
}
