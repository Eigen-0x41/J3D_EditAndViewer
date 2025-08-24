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

namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform
{
    // TODO:
    internal class StructUniformManager<UniformT> : IUniformManager<UniformT>
        where UniformT : struct//, IUniform
    {
        private static int uniqueBufferBindingIndexMaker = 0;

        private readonly string DefineName;

        private bool _disposed = false;

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
        readonly private int UniformBindingIndex;

        public int WriteDefinicator(StringBuilder builder, in int beginIndex = 0)
        {
            var membersInfo = typeof(UniformT).GetFields();
            builder.Append($"layout(std140, binding = {beginIndex}) uniform {DefineName} {{\n");
            foreach (int I in Enumerable.Range(0, membersInfo.Length))
            {
                var memberInfo = membersInfo[I];
                var typeGLSL = new TypeGLSL(memberInfo.FieldType);

                builder.Append($"  {typeGLSL.Name} {memberInfo.Name};\n");
            }
            builder.Append("};\n");

            return beginIndex + 1;
        }


        public StructUniformManager(string defineName, in UniformT uniformData)
        {
            data = uniformData;
            UniformBindingIndex = uniqueBufferBindingIndexMaker++;

            // BufferObjectの確保。
            BufferIndex = GL.GenBuffer();
            UpdateBuffer();

            // DefineNameはlocationによる修飾が無い場合に必要。
            // そうでなくともコンパイルのために必要になる。
            DefineName = defineName;
        }
        ~StructUniformManager()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            GL.DeleteBuffer(BufferIndex);
        }

        private void UpdateBuffer()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, BufferIndex);
            GL.BufferData(BufferTarget.UniformBuffer, SizeInBytes, 0, BufferUsageHint.DynamicDraw);

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, UniformBindingIndex, BufferIndex);
        }

        public void Use()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, BufferIndex);
            GL.BufferSubData(BufferTarget.UniformBuffer, 0, SizeInBytes, ref data);
        }
    }
}
