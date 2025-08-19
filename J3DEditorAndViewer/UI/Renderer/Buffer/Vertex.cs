using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using GalaxyPlantInCrystal_KEIJI.Util;
using J3DEditorAndViewer.UI.Renderer.Shader;
using J3DEditorAndViewer.ResourceServer;
// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace J3DEditorAndViewer.UI.Renderer.Buffer
{
    internal class Vertex : IBuffer
    {
        private bool _disposed;
        /// <summary>
        /// 型の要素数
        /// </summary>
        private int Size;
        /// <summary>
        /// 型の使用のされ方
        /// </summary>
        private VertexAttribPointerType VAPT;
        /// <summary>
        /// CPUでの配列内での最初の値のオフセット
        /// </summary>
        private int Offset;
        /// <summary>
        /// CPU配列での次のシェーダに渡す値へのスライド値
        /// </summary>
        private int Stride;

        readonly private int VBO;

        public Vertex(Vector3[] Verteces, BufferUsageHint BufUsageHint = BufferUsageHint.StaticDraw)
        {
            VBO = Creator(Verteces, BufUsageHint);
        }
        ~Vertex()
        {
            Dispose();
        }

        private static int Creator(Vector3[] Verteces, BufferUsageHint BufUsageHint)
        {
            int Ret = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Ret);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * Verteces.Length, Verteces, BufUsageHint);
            return Ret;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
        }

        /// <summary>
        /// </summary>
        /// <param name="Location">シェーダーでのlayout(location = X)の値</param>
        /// <returns></returns>
        public void Use(int Location)
        {
            GL.VertexAttribPointer(Location, Size, VAPT, false, Stride, Offset);
            GL.EnableVertexAttribArray(Location);
        }
    }
}
