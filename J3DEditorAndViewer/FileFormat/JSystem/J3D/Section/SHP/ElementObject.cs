using GalaxyPlantInCrystal_KEIJI.EndianBinaryStream.BinaryReaderSystem;
using J3DEditorAndViewer.FileFormat.JSystem.J3D.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J3DEditorAndViewer.FileFormat.JSystem.J3D.Section.SHP
{
    public class ElementObject
    {
        public int PositionMatrixIndex = 0;
        public int TexMatrix0Index = 0;
        public int TexMatrix1Index = 0;
        public int TexMatrix2Index = 0;
        public int TexMatrix3Index = 0;
        public int TexMatrix4Index = 0;
        public int TexMatrix5Index = 0;
        public int TexMatrix6Index = 0;
        public int TexMatrix7Index = 0;
        public int PositionIndex = 0;
        public int NBTIndex = 0;
        public int Color0Index = 0;
        public int Color1Index = 0;
        public int TexCoord0Index = 0;
        public int TexCoord1Index = 0;
        public int TexCoord2Index = 0;
        public int TexCoord3Index = 0;
        public int TexCoord4Index = 0;
        public int TexCoord5Index = 0;
        public int TexCoord6Index = 0;
        public int TexCoord7Index = 0;

        public ElementObject() { }

        public void SetIndex(EndianBinaryReaderBase br, VertexIndexAttribute attrib)
        {
            switch (attrib.AttributeType)
            {
                case GXAttribType.PositionMatrix: PositionMatrixIndex = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix0: TexMatrix0Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix1: TexMatrix1Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix2: TexMatrix2Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix3: TexMatrix3Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix4: TexMatrix4Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix5: TexMatrix5Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix6: TexMatrix6Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexMatrix7: TexMatrix7Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.Position: PositionIndex = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.NBT: NBTIndex = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.Color0: Color0Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.Color1: Color1Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord0: TexCoord0Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord1: TexCoord1Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord2: TexCoord2Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord3: TexCoord3Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord4: TexCoord4Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord5: TexCoord5Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord6: TexCoord6Index = attrib.Reader.ReadInt32(br); return;
                case GXAttribType.TexCoord7: TexCoord7Index = attrib.Reader.ReadInt32(br); return;
            }
        }
    }
}
