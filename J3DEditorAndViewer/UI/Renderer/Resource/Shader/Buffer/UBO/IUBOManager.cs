using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.UBO
{
    internal interface IUBOManager<UniformT> : IUBOCommonManager
        where UniformT : struct
    {
        public ref UniformT Data { get; }
    }
}
