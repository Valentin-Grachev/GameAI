using System.Collections.Generic;
using System.Linq;
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

