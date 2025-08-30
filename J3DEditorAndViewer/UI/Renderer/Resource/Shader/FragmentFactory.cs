// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.UBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal class FragmentFactory : IFragmentFactory
    {
        bool disposed;
        private string SourceCode;

        public IUBOCommonManager[] UniformManagers;
        public ShaderType ShaderType { get { return ShaderType.FragmentShader; } }

        public FragmentFactory(IUBOCommonManager[] uniformManagers, string sourceCode)
        {
            UniformManagers = uniformManagers;
            SourceCode = sourceCode + "\n";
        }
        public FragmentFactory(string sourceCode)
        {
            UniformManagers = new IUBOCommonManager[] { };
            SourceCode = sourceCode + "\n";
        }

        ~FragmentFactory()
        {
            if (!disposed)
            {
                throw new Exception("Dispose が呼ばれていません。");
            }
        }

        public int WriteDefinicator(StringBuilder builder, in int beginIndex)
        {
            int currentIndex = beginIndex;

            foreach (var uniform in UniformManagers)
            {
                currentIndex = uniform.WriteDefinicator(builder, currentIndex);
            }

            builder.Append(SourceCode);

            return currentIndex;
        }

        public void Dispose()
        {
            if (disposed) { return; }
            disposed = true;
            foreach (var uniform in UniformManagers)
            {
                uniform.Dispose();
            }
        }

        public void Use()
        {
            foreach (var uniform in UniformManagers)
            {
                uniform.Use();
            }
        }

    }
}
