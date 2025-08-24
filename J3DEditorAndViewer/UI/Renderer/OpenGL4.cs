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
using System.Diagnostics;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader;
using J3DEditorAndViewer.FileFormat.SectionFormat;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.VAO;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform;
using J3DEditorAndViewer.IO;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.EBO;

namespace J3DEditorAndViewer.UI.Renderer
{

    internal class OpenGL4 : IRenderer
    {
        //
        // RendererInterface
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        private static string VShaderCode = @"
out vec4 Color[2];

void main() {
  gl_Position = vec4(Position, 0.0f) * Model * View * Projection;

  Color[0] = Color0;
  Color[1] = Color1;
}
";
        private static string FShaderCode = @"
in vec4 Color[2];

out vec4 FragColor;

void main() {
  FragColor = mix(Color[0], Color[1], Mixer);
}
";

        public OpenGL4(J3DFileDialog j3d_FileDialog)
        {
            VAOManager = new StructVAOManager<VTX1Data>(j3d_FileDialog.J3DData.Model.VerTexData.GetData());
            UniformManagerProjection = new StructUniformManager<ProjectionUniform>("CoordinateUniform", new ProjectionUniform());
            VertexFactory<VTX1Data, ProjectionUniform> vfactory = new(new[] { VAOManager }, new[] { UniformManagerProjection }, VShaderCode);

            UniformManagerMixer = new StructUniformManager<MixerUniform>("MixerUniform", new MixerUniform());
            var ffactory = new FragmentFactory<MixerUniform>(new[] { UniformManagerMixer }, FShaderCode);

            VEOManager = new TriangleEBOManager(j3d_FileDialog.J3DData.Model.ShapeData.GetTriangleindexes().ToArray());

            Shader = new J3DShader(vfactory, ffactory);
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
            Shader.Dispose();
            UniformManagerProjection.Dispose();
            UniformManagerMixer.Dispose();
            VAOManager.Dispose();
            VEOManager.Dispose();
        }

        struct ProjectionUniform
        {
            public Matrix4 Model = new();
            public Matrix4 View = new();
            public Matrix4 Projection = new();

            public ProjectionUniform()
            {
            }
        }

        struct MixerUniform
        {
            public float Mixer;
        }

        private IVAOManager<VTX1Data> VAOManager;
        private IUniformManager<ProjectionUniform> UniformManagerProjection;
        private IUniformManager<MixerUniform> UniformManagerMixer;
        private IEBOManager VEOManager;
        private IShader Shader;



        // Camera
        private Vector3 cameraPosition;
        private Vector2 cameraAxsis;
        private float cameraDistance;
        public ref Vector3 CameraPosition { get { return ref cameraPosition; } }
        public ref Vector2 CameraAxsis { get { return ref cameraAxsis; } }
        public ref float CameraDistance { get { return ref cameraDistance; } }

        private void UpdateViewPosition()
        {
            Vector3 cameraPos = new Vector3(CameraPosition);
            cameraPos.X -= CameraDistance * ((float)Math.Cos(CameraAxsis.X) * (float)Math.Cos(CameraAxsis.Y));
            cameraPos.Y -= CameraDistance * ((float)Math.Sin(CameraAxsis.Y));
            cameraPos.Z -= CameraDistance * (-(float)Math.Sin(CameraAxsis.X) * (float)Math.Cos(CameraAxsis.Y));

            // カメラの位置を計算して、視点行列を適用。
            UniformManagerProjection.Data.View = Matrix4.LookAt(cameraPos, CameraPosition, Vector3.UnitY);
        }

        private void UpdateProjection(float fovY = 90.0f, float depthNear = 0.1f, float depthFar = 1000.0f)
        {
            UniformManagerProjection.Data.Projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(fovY),
                (float)Width / (float)Height,
                depthNear,
                depthFar);
        }

        public void Update(GLControl surface)
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Debug.WriteLine($"OpenGL Error: {error}");
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);
            surface.Invalidate();

            ///
            /// 

            // プログラムの指定。
            Shader.Use();

            // Uniformデータの更新とセットアップ。
            UpdateViewPosition();
            UpdateProjection();
            UniformManagerProjection.Use();

            UniformManagerMixer.Data.Mixer = 1.0f;
            UniformManagerMixer.Use();

            // 頂点データの指定。
            VAOManager.Use();
            VEOManager.Use();

            ///
            ///

            // バッファの交換
            surface.SwapBuffers();
        }
    }
}

