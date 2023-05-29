using DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public class AStar_NavAlgorithm : NavAlgorithm
    {
        

        private void PushToHeap(BinaryHeap<HeuristicVertex> heap, NavVertex vertex, NavVertex begin, NavVertex end)
        {
            HeuristicVertex heapVertex = new HeuristicVertex();
            heapVertex.navVertex = vertex;

            vertex.GridPosition(out int x, out int y);
            end.GridPosition(out int endX, out int endY);

            heapVertex.heuristic = Mathf.Abs(x - endX) + Mathf.Abs(y - endY);
            heap.Push(heapVertex);
        }


        public override List<NavVertex> FindPath(List<NavVertex> vertices, NavVertex begin, NavVertex end)
        {
            foreach (var vertex in vertices)
                vertex.ClearState();

            bool pathExists = false;

            var heap = new BinaryHeap<HeuristicVertex>();
            PushToHeap(heap, begin, begin, end);
            begin.Visit(null);

            while (!heap.IsEmpty())
            {
                NavVertex processedVertex = heap.Peek().navVertex;

                for (int i = 0; i < processedVertex.neighbourIds.Count; i++)
                {
                    NavVertex neighbourVertex = vertices[processedVertex.neighbourIds[i]];

                    if (neighbourVertex.state == NavVertex.State.Empty
                        && neighbourVertex.visitedFrom == null && neighbourVertex.id != begin.id)
                    {
                        neighbourVertex.Visit(from: processedVertex);
                        PushToHeap(heap, neighbourVertex, begin, end);
                        if (neighbourVertex.id == end.id)
                        {
                            pathExists = true;
                            break;
                        }

                        break;
                    }

                    if (i == processedVertex.neighbourIds.Count - 1) heap.Pop();
                }


                if (pathExists) return BuildPath(end);
            }

            return null;
        }




    }

}




