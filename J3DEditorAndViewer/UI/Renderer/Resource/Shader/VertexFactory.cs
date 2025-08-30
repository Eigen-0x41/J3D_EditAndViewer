// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.UBO;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.VAO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal class VertexFactory : IVertexFactory
    {
        private bool disposed;
        private string SourceCode;

        public IVAOCommonManager[] VAOManagers;
        public IUBOCommonManager[] UniformManagers;
        public ShaderType ShaderType { get { return ShaderType.VertexShader; } }

        public VertexFactory(IVAOCommonManager[] vAOManagers, IUBOCommonManager[] uniformManagers, string sourceCode)
        {
            VAOManagers = vAOManagers;
            UniformManagers = uniformManagers;
            SourceCode = sourceCode + "\n";
        }
        public VertexFactory(IVAOCommonManager[] vAOManagers, string sourceCode)
        {
            VAOManagers = vAOManagers;
            UniformManagers = new IUBOCommonManager[] { };
            SourceCode = sourceCode + "\n";
        }

        ~VertexFactory()
        {
            if (!disposed)
            {
                throw new Exception("Dispose が呼ばれていません。");
            }
        }

        public int WriteDefinicator(StringBuilder builder)
        {
            int currentIndexLocation = 0;
            foreach (var vao in VAOManagers)
            {
                currentIndexLocation = vao.WriteDefinicator(builder, currentIndexLocation);
            }

            int currentIndexBinding = 0;
            foreach (var uniform in UniformManagers)
            {
                currentIndexBinding = uniform.WriteDefinicator(builder, currentIndexBinding);
            }

            builder.Append(SourceCode);

            return currentIndexBinding;
        }

        public void Dispose()
        {
            if (disposed) { return; }
            disposed = true;
            foreach (var uniform in UniformManagers)
            {
                uniform.Dispose();
            }

            foreach (var vao in VAOManagers)
            {
                vao.Dispose();
            }
        }

        public void Use()
        {
            foreach (var vao in VAOManagers)
            {
                vao.Use();
            }

            foreach (var uniform in UniformManagers)
            {
                uniform.Use();
            }
        }
    }
}
