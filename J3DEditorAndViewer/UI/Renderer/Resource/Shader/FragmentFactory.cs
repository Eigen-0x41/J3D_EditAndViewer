// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal class FragmentFactory<UniformT> : IFragmentFactory
        where UniformT : struct
    {
        private string MethotDefinication;
        private string Version;

        public IUniformManager<UniformT>[] UniformManagers;
        public ShaderType ShaderType { get { return ShaderType.FragmentShader; } }

        public FragmentFactory(IUniformManager<UniformT>[] uniformManagers, string methotDefinication, string version = "#version 430 core")
        {
            UniformManagers = uniformManagers;
            MethotDefinication = methotDefinication + "\n";
            Version = version + "\n";
        }

        ~FragmentFactory()
        {
            Dispose();
        }

        public int WriteDefinicator(StringBuilder builder, in int beginIndex)
        {
            builder.Append(Version);
            int currentIndex = beginIndex;

            foreach (var uniform in UniformManagers)
            {
                currentIndex = uniform.WriteDefinicator(builder, currentIndex);
            }

            builder.Append(MethotDefinication);

            return currentIndex;
        }

        public void Dispose()
        {
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
