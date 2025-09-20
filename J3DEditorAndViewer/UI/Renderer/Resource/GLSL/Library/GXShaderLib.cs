using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.UI.Renderer.Resource.GLSL.Library
{
    /// <summary>
    /// main関数でusing束縛による使用を想定しています。
    /// </summary>
    internal class GXShaderLib
    {
        enum Operation
        {
            ADD = 0,
            SUBTRUCT = 1,
            GT_R8 = 8,
            EQ_R8 = 9,
            GT_GR16 = 10,
            EQ_GR16 = 11,
            GT_BGR24 = 12,
            EQ_BGR24 = 13,
            GT_8 = 14,
            EQ_8 = 15,
        }

        private static Dictionary<Operation, int> Handle = new() { };

        // "[Color, Alpha]InA": "Zero",
        // "[Color, Alpha]InB": "Konst",
        // "[Color, Alpha]InC": "TexColor",
        // "[Color, Alpha]InD": "Zero",
        // "[Color, Alpha]Op": "Add",
        // "[Color, Alpha]Bias": "Zero",
        // "[Color, Alpha]Scale": "Scale_1",
        // "[Color, Alpha]Clamp": true,
        // "[Color, Alpha]RegId": "TevPrev",
        public static string Header = @"
void TevStage00(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage01(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage02(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage03(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage04(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage05(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage06(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage07(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage08(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage09(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage10(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage11(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage12(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage13(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage14(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
void TevStage15(ivec4 A, ivec B, ivec C, ivec D, int Bias, int Scale);
";
    }
}
