using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object.Buffer.SSBO
{
    // WARNING: 実装されない可能性あり。
    internal class StructSSBOManager : ISSBOManager
    {
        public int WriteDefinicator(StringBuilder builder, IGLSLTypeTraits typeTrait, int location = 0) => throw new NotImplementedException();
        public void Dispose() => throw new NotImplementedException();
        public void Use() => throw new NotImplementedException();
    }
}
