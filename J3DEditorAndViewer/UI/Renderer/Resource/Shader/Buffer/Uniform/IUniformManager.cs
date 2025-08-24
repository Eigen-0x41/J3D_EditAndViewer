using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform
{
    internal interface IUniformManager<UniformT> : IBuffer
        where UniformT : struct
    {
        public ref UniformT Data { get; }
    }
}
