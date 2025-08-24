// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.VAO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal class VertexFactory<InT, UniformT> : IVertexFactory
        where InT : struct
        where UniformT : struct
    {
        private string Version;
        private string MethotDefinication;

        public IVAOManager<InT>[] VAOManagers;
        public IUniformManager<UniformT>[] UniformManagers;
        public ShaderType ShaderType { get { return ShaderType.VertexShader; } }

        public VertexFactory(IVAOManager<InT>[] vAOManagers, IUniformManager<UniformT>[] uniformManagers, string methotDefinication, string version = "#version 430 core")
        {
            VAOManagers = vAOManagers;
            UniformManagers = uniformManagers;
            Version = version + "\n";
            MethotDefinication = methotDefinication + "\n";
        }

        ~VertexFactory()
        {
            Dispose();
        }

        public int WriteDefinicator(StringBuilder builder)
        {
            builder.Append(Version);

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

            builder.Append(MethotDefinication);

            return currentIndexBinding;
        }

        public void Dispose()
        {
            foreach (var vao in VAOManagers)
            {
                vao.Dispose();
            }

            foreach (var uniform in UniformManagers)
            {
                uniform.Dispose();
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
