using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using MIConvexHull;
using Dynamo.Graph.Nodes;

namespace SolidConvexHull
{
    /// <summary>
    /// The HelloDynamoZeroTouch class demonstrates
    /// how to create a class in a zero touch library
    /// which creates geometry, and exposes public 
    /// methods and properties as nodes.
    /// </summary>
    public class SolidConvexHull : IGraphicItem
    {

        private SolidConvexHull()
        {

        }

        /// <summary>
        /// The Tessellate method in the IGraphicItem interface allows
        /// you to specify what is drawn when dynamo's visualization is
        /// updated.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {

        }

        /// <summary>
        /// Calculates the 3D Convex Hull of a list of points. This node is designed to pass its output data
        /// directly to the MeshToolkit.ByVerticesAndIndices node.
        /// </summary>
        /// <param name="points"></param>
        /// <returns>Convex Hull</returns>
        [NodeCategory("Create")]
        [MultiReturn(new[] { "Points", "Indices" })]
        public static Dictionary<string, List<object>> FacesAsPointsAndIndices(List<Point> points)
        {
            List<Vertex> vertices = new List<Vertex>();
            foreach (Point p in points) {
                vertices.Add(new Vertex(p.X, p.Y, p.Z));
            }
            var convexHull = ConvexHull.Create<Vertex, Face>(vertices);
            List<Face> faces = convexHull.Result.Faces.ToList();

            List<Point> flattenedList = new List<Point>();

            foreach (Face f in faces) {
                Vertex[] verts = f.Vertices;
                Point[] facePoints = new Point[3];
                for(int i = 0; i < verts.Length; i++) {
                    double x = f.Vertices[i].Position[0];
                    double y = f.Vertices[i].Position[1];
                    double z = f.Vertices[i].Position[2];
                    Point facePoint = Point.ByCoordinates(x, y, z);
                    facePoints[i] = facePoint;
                    flattenedList.Add(facePoint);
                }
            }

            // Cull duplicate points from the set
            var uniquePointsHashSet = new HashSet<Point>(flattenedList);

            // Convert back to a list
            var uniquePoints = new List<object>(uniquePointsHashSet);

            List<object> indices = new List<object>();
            foreach (Point pt in flattenedList)
            {
                indices.Add(uniquePoints.IndexOf(pt));
            }

            return new Dictionary<string, List<object>>()
            {
                { "Points", uniquePoints },
                { "Indices", indices }
            };
        }

        /// <summary>
        /// Calculates the 3D Convex Hull of a list of points. Points are returned in lists of three, with
        /// each list representing a triangular face of the convex hull.
        /// </summary>
        /// <param name="points"></param>
        /// <returns>Convex Hull</returns>
        [NodeCategory("Create")]
        public static List<Point[]> FacesAsThreePoints(List<Point> points)
        {
            List<Vertex> vertices = new List<Vertex>();
            foreach (Point p in points)
            {
                vertices.Add(new Vertex(p.X, p.Y, p.Z));
            }

            var convexHull = ConvexHull.Create<Vertex, Face>(vertices);
            List<Face> faces = convexHull.Result.Faces.ToList();

            List<Point[]> outPoints = new List<Point[]>();

            foreach (Face f in faces)
            {
                Vertex[] verts = f.Vertices;
                Point[] facePoints = new Point[3];
                for (int i = 0; i < verts.Length; i++)
                {
                    double x = f.Vertices[i].Position[0];
                    double y = f.Vertices[i].Position[1];
                    double z = f.Vertices[i].Position[2];
                    Point facePoint = Point.ByCoordinates(x, y, z);
                    facePoints[i] = facePoint;
                }
                outPoints.Add(facePoints);
            }

            return outPoints;
        }


    }
}

