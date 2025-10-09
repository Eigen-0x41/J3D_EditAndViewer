// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.FileFormat.JSystem.J3D;
using J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.SHP;
using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object.Array.VAO;
using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object.Buffer.VBO;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.VTX;

namespace J3DEditorAndViewer.UI.Renderer.Library
{
    internal class GXPrimitive : IDisposable
    {
        private bool isDisposed;
        private int vertexCount;

        public PrimitiveType GLPrimitiveType;
        public IVBOManager<Vector4> VBOMPosition;
        public IVBOManager<Vector4i> VBOMPositionMatrix;
        public IVBOManager<Vector3> VBOMNormal;
        public IVBOManager<Vector3> VBOMBinormal;
        public IVBOManager<Vector3> VBOMTangent;
        public IVBOManager<Vector4> VBOMColor0;
        public IVBOManager<Vector4> VBOMColor1;
        public IVBOManager<Vector3> VBOMTexCoord0;
        public IVBOManager<Vector3> VBOMTexCoord1;
        public IVBOManager<Vector3> VBOMTexCoord2;
        public IVBOManager<Vector3> VBOMTexCoord3;
        public IVBOManager<Vector3> VBOMTexCoord4;
        public IVBOManager<Vector3> VBOMTexCoord5;
        public IVBOManager<Vector3> VBOMTexCoord6;
        public IVBOManager<Vector3> VBOMTexCoord7;



        public GXPrimitive(VertexObjectArrays j3DVertex, Primitive j3DPrimitive)
        {
            isDisposed = false;

            switch (j3DPrimitive.Type)
            {
                case Primitive.PrimitiveType.Triangle:
                    GLPrimitiveType = PrimitiveType.Triangles;
                    break;
                case Primitive.PrimitiveType.TriangleStrip:
                    GLPrimitiveType = PrimitiveType.TriangleStrip;
                    break;
                case Primitive.PrimitiveType.TriangleFan:
                    GLPrimitiveType = PrimitiveType.TriangleFan;
                    break;
                case Primitive.PrimitiveType.Line:
                case Primitive.PrimitiveType.Point:
                case Primitive.PrimitiveType.Quad:
                default:
                    throw new InvalidOperationException();
            }
        }

        private IReadOnlyList<IVBOCommonManager> ToReadOnlyList()
        {
            return [
                VBOMPosition,
                VBOMPositionMatrix,
                VBOMNormal,
                VBOMBinormal,
                VBOMTangent,
                VBOMColor0,
                VBOMColor1,
                VBOMTexCoord0,
                VBOMTexCoord1,
                VBOMTexCoord2,
                VBOMTexCoord3,
                VBOMTexCoord4,
                VBOMTexCoord5,
                VBOMTexCoord6,
                VBOMTexCoord7
                ];
        }

        public VAOManager ToVAOManager()
        {
            return new VAOManager(ToReadOnlyList());
        }

        public void Dispose()
        {
            if (isDisposed) return;
            isDisposed = true;
            VBOMPosition.Dispose();
            VBOMPositionMatrix.Dispose();
            VBOMNormal.Dispose();
            VBOMBinormal.Dispose();
            VBOMTangent.Dispose();
            VBOMColor0.Dispose();
            VBOMColor1.Dispose();
            VBOMTexCoord0.Dispose();
            VBOMTexCoord1.Dispose();
            VBOMTexCoord2.Dispose();
            VBOMTexCoord3.Dispose();
            VBOMTexCoord4.Dispose();
            VBOMTexCoord5.Dispose();
            VBOMTexCoord6.Dispose();
            VBOMTexCoord7.Dispose();
        }
    }
}
