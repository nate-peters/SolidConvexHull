using Autodesk.DesignScript.Runtime;
using MIConvexHull;


namespace SolidConvexHull
{
    /// <summary>
    /// A vertex is a simple class that stores the postion of a point, node or vertex.
    /// </summary>
    [SupressImportIntoVM]
    [IsVisibleInDynamoLibrary(false)]
    public class Face : ConvexFace<Vertex, Face>
    {

    }
}
