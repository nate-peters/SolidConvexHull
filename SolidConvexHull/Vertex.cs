using System;
using Autodesk.DesignScript.Runtime;
using MIConvexHull;

namespace SolidConvexHull
{
    [SupressImportIntoVM]
    [IsVisibleInDynamoLibrary(false)]
    public class Vertex: IVertex
    {
        //private Vertex()
        //{

        //}

        public Vertex(double x, double y, double z, bool isHull = false)
        {
            Position = new double[] { x, y, z };
        }

        public Vertex AsHullVertex()
        {
            return new Vertex(Position[0], Position[1], Position[2], true);
        }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        /// <value>The coordinates.</value>
        public double[] Position
        {
            get;
            set;
        }
    }
}
