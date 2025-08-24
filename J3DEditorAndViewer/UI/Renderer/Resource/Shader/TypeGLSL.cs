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


namespace J3DEditorAndViewer.UI.Renderer.Resource.Shader
{
    internal class TypeGLSL
    {
        private static readonly Dictionary<Type, (string, VertexAttribPointerType, int)> Infomatons =
            new() {
                {typeof(bool)   , ("bool"  , VertexAttribPointerType.Byte       , 1    )},
                {typeof(int)    , ("int"   , VertexAttribPointerType.Int        , 1    )},
                {typeof(uint)   , ("uint"  , VertexAttribPointerType.UnsignedInt, 1    )},
                {typeof(float)  , ("float" , VertexAttribPointerType.Float      , 1    )},
                {typeof(double) , ("double", VertexAttribPointerType.Double     , 1    )},
                {typeof(Vector2), ("vec2"  , VertexAttribPointerType.Float      , 2    )},
                {typeof(Vector3), ("vec3"  , VertexAttribPointerType.Float      , 3    )},
                {typeof(Vector4), ("vec4"  , VertexAttribPointerType.Float      , 4    )},
                {typeof(Matrix2), ("mat2"  , VertexAttribPointerType.Float      , 2 * 2)},
                {typeof(Matrix3), ("mat3"  , VertexAttribPointerType.Float      , 3 * 3)},
                {typeof(Matrix4), ("mat4"  , VertexAttribPointerType.Float      , 4 * 4)},
            };

        public readonly string Name;
        public readonly VertexAttribPointerType Type;
        public readonly int LengthOfType;

        public TypeGLSL(Type type)
        {
            // NOTE: GetTryValueが遅い場合、分岐を無くしてもいいかもしれない。
            if (!Infomatons.ContainsKey(type))
            {
                throw new KeyNotFoundException("指定された型はGLSL組み込み型として登録されていません。");
            }

            var info = Infomatons[type];

            Name = info.Item1;
            Type = info.Item2;
            LengthOfType = info.Item3;
        }
    }
}
