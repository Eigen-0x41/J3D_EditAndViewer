using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Shader
{
    /// <summary>
    /// シェーダーのコンパイルや取得をするインターフェース。
    /// シェーダーコードの重複をしないようにする役目もありますが、その実装は各シェーダーにより異なります。
    /// </summary>
    internal interface IShader
    {
        /// <summary>
        /// シェーダー用ハンドル
        /// </summary>
        int Handle { get; }

        /// <summary>
        /// クラスのデストラクタが呼ばれる前に呼んでください。
        /// </summary>
        void Dispose();
        void Use();
    }
}
