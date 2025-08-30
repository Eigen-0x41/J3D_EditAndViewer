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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Diagnostics;
using System.CodeDom;
using System.DirectoryServices.ActiveDirectory;


namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.VAO
{
    internal class StructVAOManager<InT> : IVAOManager<InT>
        where InT : struct//, IVertexObject
    {
        private readonly AutoBindBuffer AutoBinder;

        private bool disposed = false;
        private bool isModified = false;

        private InT[] data;
        public InT[] Data
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
        private readonly int SizeInBytes = Marshal.SizeOf<InT>();
        /// <summary>
        /// 配列の要素数
        /// </summary>
        private int Length
        {
            get { return Data.Length; }
        }
        /// <summary>
        /// 配列のバイト数
        /// </summary>
        private int LengthInBytes
        {
            get { return Length * SizeInBytes; }
        }

        readonly private int BufferIndex;
        readonly private int ArrayIndex;

        public int WriteDefinicator(StringBuilder builder, in int beginLocation = 0)
        {
            GL.BindVertexArray(ArrayIndex);

            // var MemberInfo = VertexObjectT.MemberNames[location];
            var MembersInfo = typeof(InT).GetFields();
            foreach (int location in Enumerable.Range(beginLocation, MembersInfo.Length))
            {
                var MemberInfo = MembersInfo[location];
                var GLSLType = new TypeGLSL(MemberInfo.FieldType);

                builder.Append($"layout(location = {location}) in {GLSLType.Name} {MemberInfo.Name};\n");
                GL.VertexAttribPointer(location, GLSLType.LengthOfType, GLSLType.Type, false, SizeInBytes, Marshal.OffsetOf<InT>(MemberInfo.Name));
                GL.EnableVertexAttribArray(location);
            }

            GL.BindVertexArray(0);
            // 次の location = X を返す。
            return beginLocation + MembersInfo.Length;
        }

        public StructVAOManager(InT[] VertexData)
        {
            data = VertexData;
            isModified = true;


            // BufferObjectの確保。このクラスは構造体を利用するため1つのみ。
            BufferIndex = GL.GenBuffer();
            AutoBinder = new AutoBindBuffer(BufferTarget.ArrayBuffer, BufferIndex);
            UpdateBuffer();

            // ArrayObjectの確保。
            ArrayIndex = GL.GenVertexArray();
        }

        ~StructVAOManager()
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
            GL.BindVertexArray(0);

            AutoBinder.Dispose();
            GL.DeleteVertexArray(ArrayIndex);
            GL.DeleteBuffer(BufferIndex);
        }

        public void UpdateBuffer()
        {
            if (!isModified) { return; }
            isModified = false;
            GL.BufferData(BufferTarget.ArrayBuffer, LengthInBytes, data, BufferUsageHint.StaticDraw);
        }

        public void Use()
        {
            AutoBinder.BindOnly();
            //using var abb = AutoBinder.Use();
            UpdateBuffer();
            GL.BindVertexArray(ArrayIndex);
        }
    }
}
