// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object.UBO;
using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object.VAO;
using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.GLSL
{
    internal class VertexFactory : IVertexFactory
    {
        private bool disposed;
        private string SourceCode;

        private IVAOCommonManager[] VAOManagers;
        private IUBOCommonManager[] UniformManagers;
        private IGLSLTypeTraits GLSLTypeTrait;
        public ShaderType ShaderType { get { return ShaderType.VertexShader; } }

        public VertexFactory(IVAOCommonManager[] vAOManagers, IUBOCommonManager[] uniformManagers, IGLSLTypeTraits typeTrait, string sourceCode)
        {
            GLSLTypeTrait = typeTrait;
            VAOManagers = vAOManagers;
            UniformManagers = uniformManagers;
            SourceCode = sourceCode + "\n";
        }
        public VertexFactory(IVAOCommonManager[] vAOManagers, IGLSLTypeTraits typeTrait, string sourceCode)
        {
            GLSLTypeTrait = typeTrait;
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
                currentIndexLocation = vao.WriteDefinicator(builder, GLSLTypeTrait, currentIndexLocation);
            }

            int currentIndexBinding = 0;
            foreach (var uniform in UniformManagers)
            {
                currentIndexBinding = uniform.WriteDefinicator(builder, GLSLTypeTrait, currentIndexBinding);
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
