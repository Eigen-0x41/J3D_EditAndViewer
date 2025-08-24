//using System.Collections.
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using J3DEditorAndViewer.IO;
using J3DEditorAndViewer.FileFormat;
using J3DEditorAndViewer.UI.MainWindowSys;
using J3DEditorAndViewer.UI.Renderer;

namespace J3DEditorAndViewer
{
    public partial class MainWindow : Form
    {
        private int vtxcount;
        private List<Vector3> vertexPosition;
        private List<Vector3> vertexNormal;
        private static float _farRange = 64.0f;

        IRenderer renderer;

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

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            J3DFileDialog J3D_FileDialog = new J3DFileDialog();
            J3D_FileDialog.Open();
            SceneTreeNodeView sceneTreeNodeView = new SceneTreeNodeView(SceneTreeView, J3D_FileDialog.J3DData);
            sceneTreeNodeView.SetTree();

            renderer = new OpenGL4(J3D_FileDialog);

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


        private bool IsModelLoad = false;
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (renderer is null) return;
            renderer.Update(glControl);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (renderer is null) return;
            renderer.Update(glControl);
        }

        // private Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 0.0f);  // カメラの初期位置
        // private Vector2 cameraRotation = new Vector2(0.0f, 0.0f);  // カメラの初期回転
        // private float cameraDistance = 3.0f;  // カメラの回転軸に対する位置
        private float cameraSpeed = 0.5f;  // カメラの移動速度

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (renderer is null) return;

            if (e.KeyCode == Keys.W)
            {   // 前に移動
                renderer.CameraPosition.X += cameraSpeed * (float)Math.Cos(renderer.CameraAxsis.X);
                renderer.CameraPosition.Z -= cameraSpeed * (float)Math.Sin(renderer.CameraAxsis.X);
            }
            if (e.KeyCode == Keys.S)
            {   // 後ろに移動
                renderer.CameraPosition.X -= cameraSpeed * (float)Math.Cos(renderer.CameraAxsis.X);
                renderer.CameraPosition.Z += cameraSpeed * (float)Math.Sin(renderer.CameraAxsis.X);
            }
            if (e.KeyCode == Keys.A)
            {   // 左に移動
                renderer.CameraPosition.Z -= cameraSpeed * (float)Math.Cos(renderer.CameraAxsis.X);
                renderer.CameraPosition.X -= cameraSpeed * (float)Math.Sin(renderer.CameraAxsis.X);
            }
            if (e.KeyCode == Keys.D)
            {   // 右に移動
                renderer.CameraPosition.Z += cameraSpeed * (float)Math.Cos(renderer.CameraAxsis.X);
                renderer.CameraPosition.X += cameraSpeed * (float)Math.Sin(renderer.CameraAxsis.X);
            }
            if (e.KeyCode == Keys.Q)
            {   // 上に移動
                renderer.CameraPosition.Y += cameraSpeed;
            }
            if (e.KeyCode == Keys.E)
            {   // 下に移動
                renderer.CameraPosition.Y -= cameraSpeed;
            }

            glControl.Invalidate();  // 描画を更新
        }
        private Point BeforeMousePoint = new Point();
        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            float deltaX = e.X - BeforeMousePoint.X;
            float deltaY = e.Y - BeforeMousePoint.Y;
            BeforeMousePoint = new Point(e.X, e.Y);

            if (e.Button != MouseButtons.None)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:

                        deltaX *= 0.005f;
                        deltaY *= 0.005f;

                        renderer.CameraAxsis.X += deltaX;
                        renderer.CameraAxsis.Y += deltaY;
                        const double halfPi = Math.PI * 0.5;
                        if (Math.Abs(renderer.CameraAxsis.Y) > halfPi)
                        {
                            renderer.CameraAxsis.Y = (float)halfPi * ((renderer.CameraAxsis.Y < 0) ? -0.999f : 0.999f);
                        }
                        renderer.CameraAxsis = renderer.CameraAxsis;
                        break;
                    case MouseButtons.Right:
                        break;
                }
                glControl.Invalidate();  // 描画を更新
            }
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            if (renderer is null) return;
            renderer.Reset();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (renderer is null) return;
            renderer.Resize(glControl.Width, glControl.Height);
            renderer.Update(glControl);
        }

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            renderer.CameraDistance += -0.01f * e.Delta;

            glControl.Update();
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
