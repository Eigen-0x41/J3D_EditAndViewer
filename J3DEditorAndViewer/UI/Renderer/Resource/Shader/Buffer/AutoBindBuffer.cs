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

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer
{
    /// <summary>
    /// ブロックでのBindBuffer動作を楽に実装するためのクラス。<br/>
    /// クラスでの使用を除き、基本using束縛して使用して下さい。
    /// </summary>
    internal class AutoBindBuffer : IDisposable
    {
        bool disposed;

        public readonly BufferTarget BufferTarget;
        public readonly int BufferIndex;

        public AutoBindBuffer(BufferTarget bufferTarget, int bufferIndex)
        {
            BufferTarget = bufferTarget;
            BufferIndex = bufferIndex;
            GL.BindBuffer(BufferTarget, BufferIndex);
        }
        ~AutoBindBuffer()
        {
            if (!disposed)
            {
                throw new Exception("Dispose が呼ばれていません。");
            }
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            GL.BindBuffer(BufferTarget, 0);
        }

        public AutoBindBuffer Use()
        {
            return new AutoBindBuffer(BufferTarget, BufferIndex);
        }

        public void BindOnly()
        {
            GL.BindBuffer(BufferTarget, BufferIndex);
        }
    }
}
