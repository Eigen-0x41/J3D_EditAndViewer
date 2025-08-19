using System.IO;
using J3DEditAndViewer.FileFormat.SectionFormat;

namespace J3DEditAndViewer.FileFormat.Model_3D
{
    public interface IModel_3D
    {
        //byte HierarchyDepth { get; }
        INF1 SceneTreeData { get; }
        VTX1 VerTexData { get; }
        void Read(FileStream fs);
    }
}