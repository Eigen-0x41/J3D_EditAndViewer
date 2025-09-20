// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Object
{
    /// <summary>
    /// ブロックでのBindBuffer動作を楽に実装するためのクラス。<br/>
    /// クラスでの使用を除き、基本using束縛して使用して下さい。
    /// </summary>
    internal sealed class AutoBufferBinder : IAutoObjectBinder
    {
        static bool isUsing;
        bool disposed;

        public readonly BufferTarget BufferTarget;
        public readonly int BufferIndex;

        public AutoBufferBinder(BufferTarget bufferTarget, int bufferIndex)
        {
            isUsing = true;
            BufferTarget = bufferTarget;
            BufferIndex = bufferIndex;
            GL.BindBuffer(BufferTarget, BufferIndex);
        }
        ~AutoBufferBinder()
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
            isUsing = false;
            GL.BindBuffer(BufferTarget, 0);
        }

        public IAutoObjectBinder MoveObject()
        {
            if (disposed == true) throw new ObjectDisposedException(GetType().Name);
            disposed = true;
            return new AutoBufferBinder(BufferTarget, BufferIndex);
        }
    }
}
