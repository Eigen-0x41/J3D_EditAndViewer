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

namespace J3DEditorAndViewer.UI.Renderer.Buffer
{
    static class BufferHelper
    {
        public static float[] ToFlat(Vector3[] V1)
        {
            int VecLength = 3;
            float[] Ret = new float[V1.Length * VecLength];
            foreach (var V in V1)
            {
                foreach (int i in Enumerable.Range(0, VecLength))
                {
                    Ret[i] = V[i];
                }
            }
            return Ret;
        }
        public static float[] ToFlat(Vector4[] V1)
        {
            int VecLength = 4;
            float[] Ret = new float[V1.Length * VecLength];
            foreach (var V in V1)
            {
                foreach (int i in Enumerable.Range(0, VecLength))
                {
                    Ret[i] = V[i];
                }
            }
            return Ret;
        }
    }
    /// <summary>
    /// シェーダー、頂点情報、頂点配列情報など、OpenGLのバッファリソースにアクセスするクラスです。
    /// OpenGLには、バッファリソースをレンダリングする際に選択するのですが、そのバッファにも種類があります。
    /// このクラスを使用することで、各オブジェクトのバッファの使用方法を統一し、レンダリング時の処理を一元化します。
    /// </summary>
    internal interface IBuffer
    {
        void Dispose();

        /// <summary>
        /// </summary>
        /// <param name="Location">シェーダーでのlayout(location = X)の値</param>
        /// <returns></returns>
        void Use(int Location);
    }
}
