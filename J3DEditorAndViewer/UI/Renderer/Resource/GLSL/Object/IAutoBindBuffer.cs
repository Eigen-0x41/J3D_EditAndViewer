using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object
{
    internal interface IAutoBindBuffer : IDisposable
    {
        public IAutoBindBuffer Use();
        public void BindOnly();
    }
}
