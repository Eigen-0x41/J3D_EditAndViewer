// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using J3DEditorAndViewer.FileFormat.SectionFormat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.UBO
{
    // TODO:
    internal class StructUBOManager<UniformT> : IUBOManager<UniformT>
        where UniformT : struct//, IUniform
    {
        private readonly AutoBindBuffer AutoBinder;

        private readonly string DefineName;

        private bool disposed = false;

        private UniformT data;
        public ref UniformT Data
        {
            get { return ref data; }
        }
        /// <summary>
        /// 型Tのバイト数
        /// </summary>
        private int SizeInBytes = Marshal.SizeOf<UniformT>();

        readonly private int BufferIndex;

        public int WriteDefinicator(StringBuilder builder, in int beginIndex = 0)
        {
            var membersInfo = typeof(UniformT).GetFields();
            builder.Append($"layout(std140, binding = {beginIndex}) uniform {DefineName} {{\n");
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, beginIndex, BufferIndex);
            foreach (int I in Enumerable.Range(0, membersInfo.Length))
            {
                var memberInfo = membersInfo[I];
                var typeGLSL = new TypeGLSL(memberInfo.FieldType);

                builder.Append($"  {typeGLSL.Name} {memberInfo.Name};\n");
            }
            builder.Append("};\n");

            return beginIndex + 1;
        }


        public StructUBOManager(string defineName, in UniformT uniformData)
        {
            data = uniformData;

            // BufferObjectの確保。
            BufferIndex = GL.GenBuffer();

            AutoBinder = new AutoBindBuffer(BufferTarget.UniformBuffer, BufferIndex);
            GL.BufferData(BufferTarget.UniformBuffer, SizeInBytes, 0, BufferUsageHint.DynamicDraw);

            // DefineNameはlocationによる修飾が無い場合に必要。
            // そうでなくともコンパイルのために必要になる。
            DefineName = defineName;
        }
        ~StructUBOManager()
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
            AutoBinder.Dispose();
            GL.DeleteBuffer(BufferIndex);
        }

        private void UpdateBuffer()
        {
            GL.BufferSubData(BufferTarget.UniformBuffer, 0, SizeInBytes, ref data);
        }

        public void Use()
        {
            AutoBinder.BindOnly();
            //using var abb = AutoBinder.Use();
            UpdateBuffer();
        }
    }
}
