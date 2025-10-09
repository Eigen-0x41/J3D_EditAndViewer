// OpenTK
using J3DEditorAndViewer.FileFormat.JSystem.J3D;
using J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object.Buffer.VBO;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Library
{
    internal class GXVerticesMonolithic
    {
        private struct GXVertices
        {
            public Vector4 Position;
            public Vector4i PositionMatrix;
            public Vector3 Normal;
            public Vector3 Binormal;
            public Vector3 Tangent;
            public Vector4 Color0;
            public Vector4 Color1;
            public Vector3 TexCoord0;
            public Vector3 TexCoord1;
            public Vector3 TexCoord2;
            public Vector3 TexCoord3;
            public Vector3 TexCoord4;
            public Vector3 TexCoord5;
            public Vector3 TexCoord6;
            public Vector3 TexCoord7;
        }

        IVBOManager<GXVertices> VBOMVertices;

        public GXVerticesMonolithic(J3D2 j3DData)
        {
            // List<GXVertices> 
        }
    }
}
