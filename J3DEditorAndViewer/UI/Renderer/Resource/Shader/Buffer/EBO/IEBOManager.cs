using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.EBO
{
    internal interface IEBOManager : IBuffer
    {
        public uint[] Data { get; set; }
    }
}
