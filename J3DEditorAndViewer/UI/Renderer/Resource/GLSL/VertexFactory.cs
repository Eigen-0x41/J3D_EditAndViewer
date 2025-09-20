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

        private IVAOCommonManager VAOManager;
        private IUBOCommonManager[] UniformManagers;
        private IGLSLTypeTraits GLSLTypeTrait;
        public ShaderType ShaderType { get { return ShaderType.VertexShader; } }

        public VertexFactory(IVAOCommonManager vAOManager, IUBOCommonManager[] uniformManagers, IGLSLTypeTraits typeTrait, string sourceCode)
        {
            GLSLTypeTrait = typeTrait;
            VAOManager = vAOManager;
            UniformManagers = uniformManagers;
            SourceCode = sourceCode + "\n";
        }
        public VertexFactory(IVAOCommonManager vAOManager, IGLSLTypeTraits typeTrait, string sourceCode)
        {
            GLSLTypeTrait = typeTrait;
            VAOManager = vAOManager;
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
            VAOManager.WriteDefinicator(builder, GLSLTypeTrait);

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

            VAOManager.Dispose();

        }

        public void Use()
        {
            VAOManager.Use();


            foreach (var uniform in UniformManagers)
            {
                uniform.Use();
            }
        }
    }
}
