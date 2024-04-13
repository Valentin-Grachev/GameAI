using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{

    [System.Serializable]
    public class TransitVertex : Vertex
    {
        public static int verticesQuantity { get; private set; }
        public static void ClearIds() => verticesQuantity = 0;



        public struct Path
        {
            public Vector2[] points;
            public float length;

            public Path(Vector2 from, Vector2 to, Vector2[] points)
            {
                this.points = points;
                length = 0;
                Vector2 previousPoint = from;

                for (int i = 0; i < points.Length; i++)
                    length += Vector2.Distance(previousPoint, points[i]);

                length += Vector2.Distance(previousPoint, to);
            }
        }


        private int _id;
        public int[] neighbourVertexIds;
        public Path[] neighbourVertexPaths;

        public override int[] neighbourIds => neighbourVertexIds;

        public override int id => _id;

        private List<int> _neighbourIdsList;
        private List<Path> _pathsList;


        public override float GetEdgeWeight(int neighbourId) => _pathsList[neighbourId].length;


        public TransitVertex(Vector2 position)
        {
            this.position = position;
            _id = verticesQuantity;
            verticesQuantity++;
        }

        

        public void Bind(TransitVertex vertex, List<GridVertex> pathPoints)
        {
            _neighbourIdsList ??= new List<int>();
            _pathsList ??= new List<Path>();
            vertex._neighbourIdsList ??= new List<int>();
            vertex._pathsList ??= new List<Path>();

            _neighbourIdsList.Add(vertex.id);
            vertex._neighbourIdsList.Add(id);

            List<GridVertex> reversedPathPoints = new List<GridVertex>(pathPoints);
            reversedPathPoints.Reverse();

            List<Vector2> positionPath = new List<Vector2>(pathPoints.Count);
            foreach (var vertexPoint in pathPoints)
                positionPath.Add(vertexPoint.position);

            List<Vector2> reversedPositionPath = new List<Vector2>(reversedPathPoints.Count);
            foreach (var vertexPoint in reversedPathPoints)
                reversedPositionPath.Add(vertexPoint.position);


            Path path = new Path(
                from: this.position,
                to: vertex.position,
                points: positionPath.ToArray());

            Path reversedPath = new Path(
                from: this.position,
                to: vertex.position,
                points: reversedPositionPath.ToArray());


            _pathsList.Add(path);
            vertex._pathsList.Add(reversedPath);
        }

        public void FinishBinding()
        {
            if (_neighbourIdsList == null)
            {
                neighbourVertexIds = new int[0];
                neighbourVertexPaths = new Path[0];
            }
            else
            {
                neighbourVertexIds = _neighbourIdsList.ToArray();
                neighbourVertexPaths = _pathsList.ToArray();
            }

            
        }






        public void DrawGizmos(float vertexSize, List<TransitVertex> globalTransitVertices)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, vertexSize / 3f);

            for (int i = 0; i < neighbourVertexIds.Length; i++)
            {
                Vector2 from = position;
                Vector2 to = globalTransitVertices[neighbourVertexIds[i]].position;

                Vector2 previousPoint = from;
                var path = neighbourVertexPaths[i];

                for (int j = 0; j < path.points.Length; j++)
                {
                    Gizmos.DrawRay(previousPoint, path.points[j] - previousPoint);
                    previousPoint = path.points[j];
                }
                
                Gizmos.DrawRay(previousPoint, to - previousPoint);
            }

        }

        
    }
}
