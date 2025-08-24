// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.VAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal interface IVertexFactory : IFactory
    {
        ShaderType ShaderType { get; }
        int WriteDefinicator(StringBuilder builder);
    }
}
