using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using J3D_EditAndViewer.IO;
using J3D_EditAndViewer.FileFormat;
using J3D_EditAndViewer.UI.MainWindowSys;
using OpenTK.Graphics.OpenGL;
//using System.Collections.
using OpenTK;
using OpenTK.Graphics;
using System.Diagnostics;
//using OpenTK;

namespace J3D_EditAndViewer
{
    public partial class MainWindow : Form
    {
        private int vtxcount;
        private List<Vector3> vertexPosition;
        private List<Vector3> vertexNormal;
        private static float _farRange = 64.0f;

        

        public float FarRange
        {
            get => _farRange;
            private set
            {
                if (value < float.MaxValue && value > float.MaxValue) _farRange = value;
                if (value > float.MaxValue) _farRange = float.MaxValue;
                if (value < float.MinValue) _farRange = float.MinValue;


            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //glControl.Dock = DockStyle.Fill;  // GLControlをフォーム全体に埋め込む
            //glControl.Load += glControl_Load;
            //glControl.Resize += glControl_Resize;
            //glControl.Paint += glControl_Paint;

            // ボタンを作成してクリックイベントを追加
            //button1 = new Button();
            button1.Text = "描画開始";
            //button1.Dock = DockStyle.Top;
            button1.Click += button1_Click;

            //this.Controls.Add(glControl);
            //this.Controls.Add(button1);
        }


        void initGLControl()
        {
            GL.ClearColor(Color4.White);
            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, glControl.Size.Width, glControl.Size.Height);


            //透視射影
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4, 
                (float)glControl.Size.Width / (float)glControl.Size.Height,
                0.1f, 
                FarRange
                );
            GL.LoadMatrix(ref projection);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 modelview = Matrix4.LookAt(Vector3.UnitZ * 2, Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref modelview);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            J3DFileDialog J3D_FileDialog = new J3DFileDialog();
            J3D_FileDialog.Open();
            SceneTreeNodeView sceneTreeNodeView = new SceneTreeNodeView(SceneTreeView, J3D_FileDialog.J3DData);
            sceneTreeNodeView.SetTree();

            vertexPosition = new List<Vector3>();
            vertexNormal = new List<Vector3>();

            vertexPosition = J3D_FileDialog.J3DData.Model.VerTexData.Position;
            vertexNormal = J3D_FileDialog.J3DData.Model.VerTexData.Normal;

            Console.WriteLine($"PositionCount: {vertexPosition.Count}");
            Console.WriteLine($"NormalCount: {vertexNormal.Count}");
            IsModelLoad = true;
        }

        private void SceneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            textBox1.Text = e.Node.ImageKey;
        }

        private void GL_Panel_Resize(object sender, EventArgs e)
        {
            
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color4.Black);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // カメラの投影行列設定 (透視投影)
            float aspectRatio = (float)glControl.Width / (float)glControl.Height;
            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), aspectRatio, 0.1f, 100f);
            GL.LoadMatrix(ref projectionMatrix);

            GL.MatrixMode(MatrixMode.Modelview); // モデルビュー行列に切り替え
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            //SetProjection();
            //glControl.SwapBuffers();
            //glControl.Refresh();
        }

        private Vector3 m_CamTarget = new Vector3(0f, 0f, 0f);
        private Vector2 m_CamRotation = new Vector2(1f,1f);
        private Vector3 m_CamPosition = new Vector3(1f, 1f, 1f);
        //private Vector3 m_CamRotation = new Vector3(0f,0f,0f);
        private float m_CamDistance = 0.2f;
        private bool m_UpsideDown;
        private Matrix4 m_CamMatrix, m_SkyboxMatrix;
        private bool IsModelLoad = false;

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            //FarRange += 0.1f * e.Delta*10000;
            
            //Vector3 up;

            //if (Math.Cos(m_CamRotation.Y) < 0)
            //{
            //    m_UpsideDown = true;
            //    up = new Vector3(0.0f, -1.0f, 0.0f);
            //}
            //else
            //{
            //    m_UpsideDown = false;
            //    up = new Vector3(0.0f, 1.0f, 0.0f);
            //}

            //m_CamPosition.X = m_CamDistance * (float)Math.Cos(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);
            //m_CamPosition.Y = m_CamDistance * (float)Math.Sin(m_CamRotation.Y);
            //m_CamPosition.Z = m_CamDistance * (float)Math.Sin(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);

            //Console.WriteLine(m_CamPosition);

            //Vector3 skybox_target;
            //skybox_target.X = -(float)Math.Cos(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);
            //skybox_target.Y = -(float)Math.Sin(m_CamRotation.Y);
            //skybox_target.Z = -(float)Math.Sin(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);

            //Vector3.Add(ref m_CamPosition, ref m_CamTarget, out m_CamPosition);

            //m_CamMatrix = Matrix4.LookAt(m_CamPosition, m_CamTarget, up);
            //m_SkyboxMatrix = Matrix4.LookAt(Vector3.Zero, skybox_target, up);
            //m_CamMatrix = Matrix4.Mult(Matrix4.CreateScale(0.0001f), m_CamMatrix);


            //float delta = -((e.Delta /*/ 120f*/) * 100000.0f);
            //m_CamTarget.X += delta * (float)Math.Cos(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);
            //m_CamTarget.Y += delta * (float)Math.Sin(m_CamRotation.Y);
            //m_CamTarget.Z += delta * (float)Math.Sin(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);


            ////UpdateCamera();
            ////SetProjection();
            //var proj = Matrix4.LookAt(7.0f, 5.0f, 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            //GL.LoadMatrix(ref proj);
            //GL.MatrixMode(MatrixMode.Modelview);
            //glControl.SwapBuffers();
            //glControl.Refresh();



            ////////////float delta = ((e.Delta / 120f) * 100f);
            ////////////m_CamTarget.X += delta * (float)Math.Cos(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);
            ////////////m_CamTarget.Y += delta * (float)Math.Sin(m_CamRotation.Y);
            ////////////m_CamTarget.Z += delta * (float)Math.Sin(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);

            //////////////UpdateCamera();

            ////////////GL.MatrixMode(MatrixMode.Modelview);

            ////////////Vector3 up;

            ////////////if (Math.Cos(m_CamRotation.Y) < 0)
            ////////////{
            ////////////    m_UpsideDown = true;
            ////////////    up = new Vector3(0.0f, -1.0f, 0.0f);
            ////////////}
            ////////////else
            ////////////{
            ////////////    m_UpsideDown = false;
            ////////////    up = new Vector3(0.0f, 1.0f, 0.0f);
            ////////////}

            ////////////m_CamPosition.X += m_CamDistance * (float)Math.Cos(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);
            ////////////m_CamPosition.Y += m_CamDistance * (float)Math.Sin(m_CamRotation.Y);
            ////////////m_CamPosition.Z += m_CamDistance * (float)Math.Sin(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);

            ////////////Console.WriteLine(m_CamPosition);

            ////////////Vector3 skybox_target;
            ////////////skybox_target.X = -(float)Math.Cos(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);
            ////////////skybox_target.Y = -(float)Math.Sin(m_CamRotation.Y);
            ////////////skybox_target.Z = -(float)Math.Sin(m_CamRotation.X) * (float)Math.Cos(m_CamRotation.Y);

            //Vector3.Add(ref m_CamPosition, ref m_CamTarget, out m_CamPosition);

            ////////////m_CamMatrix = Matrix4.LookAt(m_CamPosition,m_CamTarget, up);
            //m_SkyboxMatrix = Matrix4.LookAt(Vector3.Zero, skybox_target, up);
            //m_CamMatrix = Matrix4.Mult(Matrix4.CreateScale(0.0001f), m_CamMatrix);

            ////////////GL.LoadMatrix(ref m_CamMatrix);
            //SetProjection();

            ////////////glControl.SwapBuffers();
            ////////////glControl.Refresh();




        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (!IsModelLoad) return;

            GL.Clear(ClearBufferMask.ColorBufferBit);  // 画面をクリア
            GL.LoadIdentity();

            // カメラの視点を設定（カメラの位置を変更）
            SetCameraPosition(cameraPosition);

            // 1. トライアングル面を描画
            DrawTriangles(vertexPosition.ToArray()/*, vertexNormal.ToArray()*/);

            

            // 2. 扇状（三角形扇）を描画
            //DrawFan();

            // バッファの交換
            glControl.SwapBuffers();

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.MatrixMode(MatrixMode.Modelview);


            //GL.PushMatrix();
            //GL.Color3(0f,0f,1f);
            //GL.Begin(PrimitiveType.TriangleFan);
            //for (int i = 0; i< vertexPosition.Count; i++) 
            //{

            //    GL.Vertex3(vertexPosition[i]/100000);
            //    //GL.Normal3(vertexNormal[i] / 100000);

            //}
            ////foreach (var position in vertexPosition)
            ////{
            ////    GL.Vertex3(position/100000);
            ////    GL.Normal3();
            ////}
            //GL.End();
            //GL.PushMatrix();

            //initGLControl();

            //glControl.SwapBuffers();
            //glControl.Refresh();
        }

        // 三角形面データ（トライアングル）
        // 各三角形の頂点は Vector3 配列に格納
        private Vector3[] triangleVertices = new Vector3[]
        {
            // 1つ目の三角形
            new Vector3(0.0f,  0.8f, 0.0f),   // 頂点1
            new Vector3(-0.8f, -0.8f, 0.0f),  // 頂点2
            new Vector3(0.8f, -0.8f, 0.0f),   // 頂点3

            // 2つ目の三角形
            new Vector3(0.0f,  0.8f, 0.0f),   // 頂点1
            new Vector3(-0.8f, -0.8f, 0.0f),  // 頂点2
            new Vector3(0.0f, -1.0f, 0.0f),   // 頂点3

            // 3つ目の三角形
            new Vector3(0.0f,  0.8f, 0.0f),   // 頂点1
            new Vector3(0.8f, -0.8f, 0.0f),   // 頂点2
            new Vector3(0.0f, -1.0f, 0.0f)    // 頂点3
        };

        // カメラ位置を設定するメソッド
        private void SetCameraPosition(Vector3 position)
        {
            // カメラの位置を計算して、視点行列を設定
            Matrix4 cameraMatrix = Matrix4.LookAt(position, Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref cameraMatrix);  // 行列を適用
        }

        private void DrawTriangles(Vector3[] triangleVertices)
        {
            // 頂点バッファを生成してデータを転送
            int vertexBuffer;
            GL.GenBuffers(1, out vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            // Vector3 配列から float[] 配列に変換
            float[] vertices = new float[triangleVertices.Length * 3];
            for (int i = 0; i < triangleVertices.Length; i++)
            {
                vertices[i * 3 + 0] = triangleVertices[i].X / 100f;
                vertices[i * 3 + 1] = triangleVertices[i].Y / 100f;
                vertices[i * 3 + 2] = triangleVertices[i].Z / 100f;
                //Debug.WriteLine(vertices[i]);
            }

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            // 赤色に設定
            GL.Color3(1.0f, 0.0f, 0.0f);  // 赤色

            // 頂点データを指定
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            // 三角形を描画
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, triangleVertices.Length);
        }

        private void DrawTriangles(Vector3[] triangleVertices, Vector3[] triangleNormals)
        {
            // 頂点バッファを生成し、データを転送
            if (vertexBuffer == 0)
            {
                GL.GenBuffers(1, out vertexBuffer);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(triangleVertices.Length * sizeof(float)), triangleVertices, BufferUsageHint.StaticDraw);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(triangleVertices.Length * sizeof(float)), triangleVertices, BufferUsageHint.StaticDraw);
            }

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            // シェーディングを設定（ライト方向と法線計算）
            for (int i = 0; i < triangleVertices.Length; i += 3)
            {
                if (i == 3) i = 2;
                Vector3 A = triangleVertices[i] / 1f;
                Vector3 B = triangleVertices[i + 1] / 1f;
                Vector3 C = triangleVertices[i + 2] / 1f;

                Vector3 normal = triangleNormals[i / 3] / 1f;
                Vector3 lightDirection = (lightPosition - A).Normalized();

                // 法線とライト方向の内積をシェーディングに使う
                float intensity = Math.Max(Vector3.Dot(normal, lightDirection), 0.0f);

                GL.Color3(intensity, 0.0f, 0.0f);  // 赤色シェーディング

                //GL.DrawArrays(PrimitiveType.Triangles, i, 3);
            }

            Vector3[] vertices = new Vector3[triangleVertices.Length];
            for (int a = 0; a < triangleVertices.Length; a++) 
            {
                vertices[a] = triangleVertices[a]/1000f;
            }

            //int vertexBuffer;
            GL.GenBuffers(1, out vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vertices.Length);

            //for (int i = 0; i < triangleVertices.Length; i += 3)
            //{
            //    if (i == 3) i = 2;
            //    // 三角形の3頂点を取得
            //    Vector3 A = triangleVertices[i] / 10000f;
            //    Vector3 B = triangleVertices[i + 1] / 10000f;
            //    Vector3 C = triangleVertices[i + 2] / 10000f;

            //    // 法線ベクトルを計算
            //    Vector3 normal = triangleNormals[i/3] / 10000f;

            //    // ライトの方向ベクトルを計算
            //    Vector3 lightDirection = (lightPosition - A).Normalized();

            //    // 法線とライトの内積を計算してシェーディング
            //    float intensity = Math.Max(Vector3.Dot(normal, lightDirection), 0.0f);

            //    // 法線の方向に基づいて色を設定
            //    GL.Color3(intensity, 0.0f, 0.0f); // 赤色を基調にシェーディング

            //    // 三角形を描画
            //    float[] vertices = new float[]
            //    {
            //        A.X, A.Y, A.Z,
            //        B.X, B.Y, B.Z,
            //        C.X, C.Y, C.Z
            //    };

            //    int vertexBuffer;
            //    GL.GenBuffers(1, out vertexBuffer);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            //    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            //    GL.EnableClientState(ArrayCap.VertexArray);
            //    GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
            //    GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 3);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            glControl.Invalidate();
        }

        private Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 3.0f);  // カメラの初期位置
        private float cameraSpeed = 0.5f;  // カメラの移動速度
        private Vector3 lightPosition = new Vector3(0.0f, 2.0f, 2.0f);  // ライトの位置
        private int vertexBuffer;

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                cameraPosition.Z -= cameraSpeed;  // 前に移動
            }
            if (e.KeyCode == Keys.S)
            {
                cameraPosition.Z += cameraSpeed;  // 後ろに移動
            }
            if (e.KeyCode == Keys.A)
            {
                cameraPosition.X -= cameraSpeed;  // 左に移動
            }
            if (e.KeyCode == Keys.D)
            {
                cameraPosition.X += cameraSpeed;  // 右に移動
            }
            if (e.KeyCode == Keys.Q)
            {
                cameraPosition.Y += cameraSpeed;  // 上に移動
            }
            if (e.KeyCode == Keys.E)
            {
                cameraPosition.Y -= cameraSpeed;  // 下に移動
            }

            glControl.Invalidate();  // 描画を更新
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void SetProjection()
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.MatrixMode(MatrixMode.Projection);
            float h = 4.0f, w = h * glControl.AspectRatio;
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 2.0f);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
        }

    }
}
