// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.EBO
{
    internal class TriangleEBOManager : IEBOManager
    {
        private bool _disposed = false;
        private bool isModified = false;

        public uint[] data;
        public uint[] Data
        {
            get { return data; }
            set
            {
                isModified |= true;
                data = value;
            }
        }

        readonly private int BufferIndex;

        public TriangleEBOManager(uint[] FaceIndexes)
        {
            data = FaceIndexes;
            isModified = true;

            // 頂点インデックスバッファを生成
            BufferIndex = GL.GenBuffer();
            UpdateBuffer();
        }

        public void Dispose()
        {
            if (_disposed) return;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(BufferIndex);
        }

        public void Use()
        {
            UpdateBuffer();
            GL.DrawElements(PrimitiveType.Triangles, Data.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void UpdateBuffer()
        {
            if (!isModified) { return; }
            // GL.GenBuffers(1, out VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferIndex);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Data.Length * sizeof(uint), Data, BufferUsageHint.StaticDraw);
        }

        public int WriteDefinicator(StringBuilder builder, in int beginIndex = 0)
        {
            // シェーダコードに関わらないため実装しません。
            throw new NotImplementedException();
        }
    }
}
