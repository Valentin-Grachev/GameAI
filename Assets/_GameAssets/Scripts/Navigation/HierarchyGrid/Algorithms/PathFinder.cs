using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public static class PathFinder
    {
        public static bool FindPathBFS<V>(List<V> graph, V start, V finish, out List<V> path) 
            where V : Vertex
        {
            foreach (var vertex in graph)
                vertex.ClearState();

            bool pathExists = false;

            var queue = new Queue<Vertex>();
            queue.Enqueue(start);
            start.Visit(null);

            while (queue.Count > 0)
            {
                Vertex processedVertex = queue.Dequeue();

                foreach (var neighbourId in processedVertex.neighbourIds)
                {
                    if (neighbourId == -1) continue;
                    Vertex neighbourVertex = graph[neighbourId];

                    if (!neighbourVertex.visited)
                    {
                        neighbourVertex.Visit(from: processedVertex);
                        queue.Enqueue(neighbourVertex);
                        if (neighbourVertex == finish)
                        {
                            pathExists = true;
                            break;
                        }
                    }

                }

                if (pathExists) break;
            }


            path = pathExists ? BuildPath(finish, start) : null;
            return pathExists;
        }

        public static bool FindPathDijkstra<V>(List<V> graph, V start, V finish, out List<V> path)
            where V : Vertex
        {
            float infinity = 1000000f;
            bool pathExists = false;

            List<float> vertexMarks = new List<float>(graph.Count);
            List<int> tempMarksVertexIds = new List<int>(graph.Count);


            for (int i = 0; i < graph.Count; i++)
            {
                vertexMarks.Add(infinity);
                tempMarksVertexIds.Add(i);
            }
                

            var currentVertex = start;
            vertexMarks[currentVertex.id] = 0f;
            tempMarksVertexIds.Remove(currentVertex.id);

            while (currentVertex != finish)
            {
                foreach (var neighbourId in currentVertex.neighbourIds)
                {
                    float currentMark = vertexMarks[neighbourId];
                    float enterMark = vertexMarks[currentVertex.id] + currentVertex.GetEdgeWeight(neighbourId);

                    if (currentMark < enterMark)
                    {
                        vertexMarks[neighbourId] = currentMark;
                        graph[neighbourId].Visit(from: currentVertex);
                    }
                }

                int minTempMarkVertexId = tempMarksVertexIds[0];
                float minMark = vertexMarks[minTempMarkVertexId];

                foreach (var tempMarkVertexId in tempMarksVertexIds)
                    if (vertexMarks[tempMarkVertexId] < minMark)
                    {
                        minTempMarkVertexId = tempMarkVertexId;
                        minMark = vertexMarks[tempMarkVertexId];
                    }
                      
                if (Mathf.Approximately(minMark, infinity)) break;

                currentVertex = graph[minTempMarkVertexId];
                tempMarksVertexIds.Remove(currentVertex.id);
            }

            pathExists = currentVertex == finish;

            path = pathExists ? BuildPath(finish, start) : null;
            return pathExists;
        }



        private static List<V> BuildPath<V>(V finish, V start) where V : Vertex
        {
            var path = new List<V>();
            V currentVertex = finish;

            while (currentVertex != start)
            {
                path.Add(currentVertex);
                Debug.Log($"Path add: {currentVertex.position}");
                currentVertex = (V)currentVertex.visitedFrom;
            }

            path.Add(currentVertex);
            Debug.Log($"Path add: {currentVertex.position}");
            path.Reverse();

            return path;
        }


    }
}

