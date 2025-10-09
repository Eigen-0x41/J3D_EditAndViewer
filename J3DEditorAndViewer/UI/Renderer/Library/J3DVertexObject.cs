using J3DEditorAndViewer.FileFormat.JSystem.J3D;
using J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.SHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Library
{
    internal class J3DVertexObject : IDisposable
    {
        public List<GXPrimitive> primitives;

        public J3DVertexObject(J3D2 j3d)
        {
            primitives = new();

            foreach (ShapeData shapeData in j3d.SHP.ShapeDatas)
            {
                foreach (Primitive primitive in shapeData.Primitives)
                {
                    primitives.Add(new GXPrimitive(j3d.VTX.VertexObjectArrays, primitive));
                }
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < primitives.Count; i++)
            {
                primitives[i].Dispose();
            }
        }
    }
}
