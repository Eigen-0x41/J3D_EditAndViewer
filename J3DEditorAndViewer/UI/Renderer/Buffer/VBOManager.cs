using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace J3DEditorAndViewer.UI.Renderer.Buffer
{
    internal class VBOManager<T> where T : struct
    {
        private bool _disposed = false;
        private bool isModified = false;

        private T[] data;
        public T[] Data
        {
            get { return data; }
            set
            {
                isModified |= true;
                data = value;
            }
        }
        /// <summary>
        /// 型Tのバイト数
        /// </summary>
        public readonly int SizeInBytes = Marshal.SizeOf<T>();
        /// <summary>
        /// 配列の要素数
        /// </summary>
        public int Length
        {
            get { return Data.Length; }
            private set { }
        }
        /// <summary>
        /// 配列のバイト数
        /// </summary>
        public int LengthInBytes
        {
            get { return Length * SizeInBytes; }
            private set { }
        }
        /// <summary>
        /// 型の使用のされ方
        /// </summary>
        private VertexAttribPointerType vertexAttribPtrType;
        public VertexAttribPointerType VertexAttribPtrType
        {
            get { return vertexAttribPtrType; }
            set
            {
                isModified |= true;
                vertexAttribPtrType = value;
            }
        }
        private BufferUsageHint bufUsageHint;
        public BufferUsageHint BufUsageHint
        {
            get { return bufUsageHint; }
            set
            {
                isModified |= true;
                bufUsageHint = value;
            }
        }
        /// <summary>
        /// CPUでの配列内での最初の値のオフセット
        /// </summary>
        private int Offset;

        readonly private int VBO;

        public VBOManager(T[] Data, BufferUsageHint BufUsageHint = BufferUsageHint.StaticDraw)
        {
            this.Data = Data;
            VBO = GL.GenBuffer();
            this.BufUsageHint = BufUsageHint;
        }
        ~VBOManager()
        {
            Dispose();
        }

        private void UpdateBufferData()
        {
            if (!isModified) return;
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, LengthInBytes, Data, BufUsageHint);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
        }

        /// <summary>
        /// シェーダー内の'layout (location = X)'修飾に対する値の配置。
        /// </summary>
        /// <param name="Location">X</param>
        /// <param name="FieldName"></param>
        /// <param name="Normalized"></param>
        public void SetLocation(int Location, string FieldName, bool Normalized = false)
        {
            GL.VertexAttribPointer(Location, Length, VertexAttribPtrType, Normalized, SizeInBytes, Marshal.OffsetOf<T>(FieldName));
        }

        /// <summary>
        /// </summary>
        /// <param name="Location">シェーダーでのlayout(location = X)の値</param>
        /// <returns></returns>
        public void Use(int Location, string FieldName, bool Normalized = false)
        {
            UpdateBufferData(); // このプロジェクトはエディターであるため必要。
            GL.EnableVertexAttribArray(Location);
        }
    }
}
