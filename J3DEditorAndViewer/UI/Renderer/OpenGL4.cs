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

namespace J3DEditorAndViewer.UI.Renderer
{

    internal class OpenGL4 : IRenderer
    {
        //
        // RendererInterface
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public void Update(GLControl Surface)
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Debug.WriteLine($"OpenGL Error: {error}");
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);  // 画面をクリア
                                                       // GL.LoadIdentity();
            Surface.Invalidate();

            TestUpdate();

            // バッファの交換
            Surface.SwapBuffers();
            return;
        }
        public void Reset()
        {
            return;
        }
        public void Resize(int _Width, int _Height)
        {
            Width = _Width;
            Height = _Height;
            GL.Viewport(0, 0, Width, Height);
        }
        public void Dispose()
        {
            TestDispos();
        }

        //
        // Test
        ShaderInterface TestShader = new DummyShader();
        int TestVAO = 0;
        int TestVBO = 0;
        int TestVBOC = 0;
        int TestVEO = 0;
        private Vector3[] TestVerteces = new Vector3[]
        {
            new Vector3( 0.5f,  0.5f, 0.0f),  // top right
            new Vector3( 0.5f, -0.5f, 0.0f), // bottom right
            new Vector3(-0.5f, -0.5f, 0.0f), // bottom left
            new Vector3(-0.5f,  0.5f, 0.0f),// top left
        };
        private Vector3[] TestVertecesColor = new Vector3[]
        {
            new Vector3( 0.0f,  0.5f, 0.5f),  // top right
            new Vector3( 0.5f,  0.0f, 0.0f), // bottom right
            new Vector3( 0.0f,  0.5f, 0.0f), // bottom left
            new Vector3( 0.0f,  0.0f, 0.5f) // top left
        };
        uint[] TestFaces = {  // note that we start from 0!
               //0, 1, 3,   // first triangle
               1, 2, 3    // second triangle
        };
        // Test end

        /// <summary>
        /// 頂点配列オブジェクトの作成。
        /// </summary>
        /// <returns></returns>
        private int CreateVAO()
        {
            return GL.GenVertexArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Location">シェーダーでのlayout(location = X)の値</param>
        /// <param name="Size">型の要素数</param>
        /// <param name="VAPT"></param>
        /// <param name="Offset">CPUでの配列内での最初の値のオフセット</param>
        /// <param name="Stride">CPU配列での次のシェーダに渡す値へのスライド値</param>
        /// <param name="BufObj">参照元のバッファデータ</param>
        /// <returns></returns>
        private int EnrollVAO(int Location, int Size, VertexAttribPointerType VAPT, int Offset, int Stride, int BufObj)
        {
            GL.BindVertexArray(BufObj);
            GL.VertexAttribPointer(Location, Size, VAPT, false, Stride, Offset);
            GL.EnableVertexAttribArray(Location);
            return Stride;
        }

        /// <summary>
        /// 生の頂点情報による頂点バッファオブジェクトの作成。
        /// </summary>
        /// <param name="RawVertices"></param>
        /// <returns></returns>
        private int CreateVBO(float[] RawVertices)
        {
            if (RawVertices.Length % 3 != 0)
            {
                return 0;
            }
            // 頂点バッファを生成してデータを転送
            int VertexBufferObject = GL.GenBuffer();
            // GL.GenBuffers(1, out VertexBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, RawVertices.Length * sizeof(float), RawVertices, BufferUsageHint.StaticDraw);
            return VertexBufferObject;
        }

        /// <summary>
        /// Vector3による頂点バッファオブジェクトを作成
        /// </summary>
        /// <param name="TriangleVertices"></param>
        /// <returns></returns>
        private int CreateVBO(Vector3[] TriangleVertices)
        {
            // Vector3 配列から float[] 配列に変換
            float[] vertices = new float[TriangleVertices.Length * Vector3.SizeInBytes];
            for (int i = 0; i < TriangleVertices.Length; i++)
            {
                vertices[(i * 3) + 0] = TriangleVertices[i].X;
                vertices[(i * 3) + 1] = TriangleVertices[i].Y;
                vertices[(i * 3) + 2] = TriangleVertices[i].Z;
                Debug.WriteLine(vertices[i]);
            }

            return CreateVBO(vertices);
        }

        /// <summary>
        /// 三角面要素バッファオブジェクトを作成
        /// </summary>
        /// <param name="Vertices"></param>
        /// <param name="TriangleFaceIndexes"></param>
        /// <returns></returns>
        private int CreateVEO(uint[] TriangleFaceIndexes)
        {
            // 頂点インデックスバッファを生成してデータを転送
            int VertexElementObject = GL.GenBuffer();
            // GL.GenBuffers(1, out VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VertexElementObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, TriangleFaceIndexes.Length * sizeof(uint), TriangleFaceIndexes, BufferUsageHint.StaticDraw);
            return VertexElementObject;
        }

        private void TestInit()
        {
            TestVAO = CreateVAO();
            TestVBO = CreateVBO(TestVerteces);
            EnrollVAO(0, 3, VertexAttribPointerType.Float, 0, 3 * sizeof(float), TestVBO);
            TestVBOC = CreateVBO(TestVertecesColor);
            EnrollVAO(1, 3, VertexAttribPointerType.Float, 0, 3 * sizeof(float), TestVBOC);
            TestVEO = CreateVEO(TestFaces);
            // TestShader = new DummyShader();
            TestShader.Use();
        }

        private void TestDispos()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.UseProgram(0);

            GL.DeleteBuffer(TestVEO);
            GL.DeleteBuffer(TestVBOC);
            GL.DeleteBuffer(TestVBO);
            GL.DeleteVertexArray(TestVAO);
            TestShader.Dispose();
            return;
        }

        double[] timeValue = { 0, Math.PI / 3, -Math.PI / 3 };
        void TestUpdate()
        {
            TestShader.Use();
            foreach (var i in Enumerable.Range(0, timeValue.Length))
            {
                timeValue[i] = (timeValue[i] >= Math.PI) ? 0.0d : timeValue[i] + 0.001d;
            }
            float[] OurAlpha = {
                (float)Math.Sin(timeValue[0]),
                (float)Math.Sin(timeValue[1]),
                (float)Math.Sin(timeValue[2])};
            int vertexColorLocation = GL.GetUniformLocation(TestShader.Handle, "OurAlpha");
            GL.Uniform3(vertexColorLocation, OurAlpha[0], OurAlpha[1], OurAlpha[2]);

            GL.BindVertexArray(TestVAO);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // C3 
            GL.DrawElements(PrimitiveType.Triangles, TestFaces.Length, DrawElementsType.UnsignedInt, 0);
        }

        public RendererTest()
        {
            TestInit();
            return;
        }
    };
}
}
