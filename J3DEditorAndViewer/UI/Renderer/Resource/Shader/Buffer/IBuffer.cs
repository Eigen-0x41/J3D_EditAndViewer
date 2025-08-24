using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer
{
    internal interface IBuffer : IFactory
    {
        int WriteDefinicator(StringBuilder builder, in int beginIndex = 0);
    }
}
