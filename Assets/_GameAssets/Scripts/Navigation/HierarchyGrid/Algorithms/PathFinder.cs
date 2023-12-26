using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public static class PathFinder
    {
        public static bool FindPathBFS(List<GridVertex> graph, GridVertex start, GridVertex finish, out List<Vector2> path)
        {
            foreach (var vertex in graph)
                vertex.ClearState();

            bool pathExists = false;

            var queue = new Queue<GridVertex>();
            queue.Enqueue(start);
            start.Visit(null);

            while (queue.Count > 0)
            {
                GridVertex processedVertex = queue.Dequeue();

                foreach (var neighbourId in processedVertex.neighbourIds)
                {
                    if (neighbourId == -1) continue;
                    GridVertex neighbourVertex = graph[neighbourId];

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

            if (pathExists)
            {
                path = BuildPath(finish, start);
                return true;
            }

            path = null;
            return false;
        }

        private static List<Vector2> BuildPath(Vertex finish, Vertex start)
        {
            var path = new List<Vector2>();
            Vertex currentVertex = finish;

            while (currentVertex != start)
            {
                path.Add(currentVertex.position);
                Debug.Log($"Path add: {currentVertex.position}");
                currentVertex = currentVertex.visitedFrom;
            }

            path.Add(currentVertex.position);
            Debug.Log($"Path add: {currentVertex.position}");
            path.Reverse();
            return path;
        }


    }
}

