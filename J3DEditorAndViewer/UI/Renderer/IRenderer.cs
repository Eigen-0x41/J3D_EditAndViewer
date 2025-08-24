// OpenTK
using J3DEditorAndViewer.FileFormat.SectionFormat;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.Uniform;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.VAO;
using J3DEditorAndViewer.UI.Renderer.Resource.Shader.Buffer.EBO;
using OpenTK.GLControl;
using OpenTK.Mathematics;
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace J3DEditorAndViewer.UI.Renderer
{
    internal interface IRenderer
    {
        int Width { get; }
        int Height { get; }

        // Camera
        ref Vector3 CameraPosition { get; }
        ref Vector2 CameraAxsis { get; }
        ref float CameraDistance { get; }

        /// <summary>
        /// DrowとSwapBuffer
        /// </summary>
        void Update(GLControl Surface);
        /// <summary>
        /// 全てのオブジェクト情報などを初期化し、再読み込みします。
        /// </summary>
        void Reset();
        /// <summary>
        /// サイズを所定の大きさにします。
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        void Resize(int Width, int Height);
        /// <summary>
        /// 終了時には必ず呼んで下さい。
        /// </summary>
        void Dispose();

    }
}
