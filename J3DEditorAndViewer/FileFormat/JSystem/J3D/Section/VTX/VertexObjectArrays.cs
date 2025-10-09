// OpenTK
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//
using GalaxyPlantInCrystal_KEIJI.EndianBinaryStream.BinaryReaderSystem;
using GalaxyPlantInCrystal_KEIJI.IO.BinarySystem.Util;
using J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.INF;
using J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.VTX.Color;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.VTX
{
    /// <summary>
    /// 頂点データを保持するクラス。
    /// ただし、マテリアルによって使用する属性に差があるため、頂点数と同じ数にはなりません。
    /// </summary>
    public class VertexObjectArrays
    {

        private List<Matrix4> positionMatrix = new(); // 使用例が無い。
        public List<Matrix4> PositionMatrix => positionMatrix;

        private List<Matrix2>[] texMatrix = [
            new(),new(),new(),new(),
            new(),new(),new(),new(),
        ]; // 使用例が無い。
        public List<Matrix2> TexMatrix0 => texMatrix[0];
        public List<Matrix2> TexMatrix1 => texMatrix[1];
        public List<Matrix2> TexMatrix2 => texMatrix[2];
        public List<Matrix2> TexMatrix3 => texMatrix[3];
        public List<Matrix2> TexMatrix4 => texMatrix[4];
        public List<Matrix2> TexMatrix5 => texMatrix[5];
        public List<Matrix2> TexMatrix6 => texMatrix[6];
        public List<Matrix2> TexMatrix7 => texMatrix[7];

        private List<Vector4> position = new();
        public List<Vector4> Position => position;

        private List<Vector3> normal = new();
        private List<Vector3> binormal = new(); // 使用例が無い。
        private List<Vector3> tangent = new();  // 使用例が無い。
        public List<Vector3> Normal => normal;
        public List<Vector3> Binormal => binormal; // 使用例が無い。
        public List<Vector3> Tangent => tangent;  // 使用例が無い。

        private List<Vector4i>[] color = [
        new(),new(),
        ]; // あまり使用されない。
        public List<Vector4i> Color0 => color[0];
        public List<Vector4i> Color1 => color[1];


        private List<Vector3>[] texCoord = [
            new(),new(),new(),new(),
            new(),new(),new(),new(),
        ]; // 3以降はあまり使用されない。
        public List<Vector3> TexCoord0 => texCoord[0];
        public List<Vector3> TexCoord1 => texCoord[1];
        public List<Vector3> TexCoord2 => texCoord[2];
        public List<Vector3> TexCoord3 => texCoord[3];
        public List<Vector3> TexCoord4 => texCoord[4];
        public List<Vector3> TexCoord5 => texCoord[5];
        public List<Vector3> TexCoord6 => texCoord[6];
        public List<Vector3> TexCoord7 => texCoord[7];


        public VertexObjectArrays(EndianBinaryReaderBase br, in ISectionHeader vTXHeader, in VTXDataHeader vTXSectionHeader, in IINFVertex iNFVertex)
        {
            var attributes = VertexAttribute.ReadVertexAttributes(br, vTXHeader.BeginPosition, vTXSectionHeader);

            foreach (var attribute in attributes)
            {
                switch (attribute.Type)
                {
                    case Types.GXAttribType.PositionMatrix:
                    case Types.GXAttribType.TexMatrix0:
                    case Types.GXAttribType.TexMatrix1:
                    case Types.GXAttribType.TexMatrix2:
                    case Types.GXAttribType.TexMatrix3:
                    case Types.GXAttribType.TexMatrix4:
                    case Types.GXAttribType.TexMatrix5:
                    case Types.GXAttribType.TexMatrix6:
                    case Types.GXAttribType.TexMatrix7:
                    case Types.GXAttribType.NBT0: // 現時点では使用例が無いため、実装するべきではないと判断。
                        throw new NotImplementedException();

                    case Types.GXAttribType.Position:
                        ReadPosition(br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.NBT: // or Nromal
                        ReadNBT(br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.Color0:
                        ReadColor(0, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.Color1:
                        ReadColor(1, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord0:
                        ReadTexCoord(0, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord1:
                        ReadTexCoord(1, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord2:
                        ReadTexCoord(2, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord3:
                        ReadTexCoord(3, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord4:
                        ReadTexCoord(4, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord5:
                        ReadTexCoord(5, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord6:
                        ReadTexCoord(6, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                    case Types.GXAttribType.TexCoord7:
                        ReadTexCoord(7, br, attribute, vTXHeader.BeginPosition, vTXSectionHeader, iNFVertex);
                        break;
                }
            }
        }
        private void ReadPosition(EndianBinaryReaderBase br, in VertexAttribute attribute, in long beginPos, in VTXDataHeader vTXDataHeader, in IINFVertex iNFVertex)
        {
            var dataHeader = vTXDataHeader.ArrayDataPair[attribute.Type];

            Debug.WriteLine($"BaseStream.Position: 0x{br.BaseStream.Position:X}");
            Debug.WriteLine($"VTX.Position: 0x{beginPos + dataHeader.Offset:X}\n");
            br.BaseStream.Seek(beginPos + dataHeader.Offset, SeekOrigin.Begin);
            var values = attribute.ReadGXCompornent(br, dataHeader.LengthInByte);
            foreach (var value in values)
            {
                position.Add(new(value[0], value[1], value.ElementAtOrDefault(2), 1.0f));
            }
        }
        private void ReadNBT(EndianBinaryReaderBase br, in VertexAttribute attribute, in long beginPos, in VTXDataHeader vTXDataHeader, in IINFVertex iNFVertex)
        {
            var dataHeader = vTXDataHeader.ArrayDataPair[attribute.Type];

            Debug.WriteLine($"BaseStream.Position: 0x{br.BaseStream.Position:X}");
            Debug.WriteLine($"VTX.NBT: 0x{beginPos + dataHeader.Offset:X}\n");
            br.BaseStream.Seek(beginPos + dataHeader.Offset, SeekOrigin.Begin);
            var values = attribute.ReadGXCompornent(br, dataHeader.LengthInByte);
            foreach (var value in values)
            {
                normal.Add(new(value[0], value[1], value[2]));
                if (value.Length > 3)
                {
                    binormal.Add(new(value[3], value[4], value[5]));
                    tangent.Add(new(value[6], value[7], value[8]));
                }
            }
        }
        private void ReadColor(in int ColorID, EndianBinaryReaderBase br, in VertexAttribute attribute, in long beginPos, in VTXDataHeader vTXDataHeader, in IINFVertex iNFVertex)
        {
            var dataHeader = vTXDataHeader.ArrayDataPair[attribute.Type];
            var reader = IColor.GetReader(br, attribute);

            int colorsLength = dataHeader.LengthInByte / reader.SizeOfBytes;

            Debug.WriteLine($"BaseStream.Position: 0x{br.BaseStream.Position:X}");
            Debug.WriteLine($"VTX.Color: 0x{beginPos + dataHeader.Offset:X}\n");
            br.BaseStream.Seek(beginPos + dataHeader.Offset, SeekOrigin.Begin);
            for (int i = 0; i < colorsLength; i++)
            {
                color[ColorID].Add(reader.Read(br));
            }
        }
        private void ReadTexCoord(in int TexID, EndianBinaryReaderBase br, in VertexAttribute attribute, in long beginPos, in VTXDataHeader vTXDataHeader, in IINFVertex iNFVertex)
        {
            var dataHeader = vTXDataHeader.ArrayDataPair[attribute.Type];

            Debug.WriteLine($"BaseStream.Position: 0x{br.BaseStream.Position:X}");
            Debug.WriteLine($"VTX.TexCoord: 0x{beginPos + dataHeader.Offset:X} \n");
            br.BaseStream.Seek(beginPos + dataHeader.Offset, SeekOrigin.Begin);
            var values = attribute.ReadGXCompornent(br, dataHeader.LengthInByte);
            foreach (var value in values)
            {
                texCoord[TexID].Add(new(value[0], value.ElementAtOrDefault(1), 1.0f));
            }
        }
    }
}
