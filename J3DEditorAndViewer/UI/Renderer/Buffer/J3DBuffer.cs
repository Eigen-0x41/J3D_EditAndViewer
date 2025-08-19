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
using J3DEditorAndViewer.UI.Renderer.Shader;

namespace J3DEditorAndViewer.UI.Renderer.Buffer
{

    internal class J3DBuffer : IBuffer
    {
        private bool _disposed = false;
        private readonly int VAO;
        private List<VertexBuffer> VBOList;
        private List<ElementBuffer> EVBOList;
        private IShader Shader;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            foreach (var VBO in VBOList)
            {
                VBO.Dispose();
            }
            foreach (var EVBO in EVBOList)
            {
                EVBO.Dispose();
            }
            GL.DeleteVertexArray(VAO);
            Shader.Dispose();
            return;
        }


        /// <summary>
        /// uniform修飾子やSSBOなどの変数の初期化。
        /// </summary>
        private void ShaderValueSet() { }

        public void Use()
        {
            Shader.Use();
            ShaderValueSet();
            GL.BindVertexArray(VAO);
            // VBOによる描画は汎用性がないため実装しません。
            foreach (var i in EVBOList)
            {
                i.Use();
            }
        }
    }
}
