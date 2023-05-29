using System.Collections.Generic;
using System;
using DataStructures;

namespace VG.GameAI.Navigation2D
{
    public class Greedy_NavAlgorithm : NavAlgorithm
    {
        private struct HeapVertex : IComparable
        {
            public NavVertex navVertex;
            public float distanceToDestination;

            public int CompareTo(HeapVertex other)
                => distanceToDestination.CompareTo(other.distanceToDestination);

            public int CompareTo(object obj)
                => distanceToDestination.CompareTo(((HeapVertex)obj).distanceToDestination);
        }

        private void PushToHeap(BinaryHeap<HeapVertex> heap, NavVertex vertex, NavVertex end)
        {
            HeapVertex heapVertex = new HeapVertex();
            heapVertex.navVertex = vertex;
            heapVertex.distanceToDestination = NavVertex.Distance(vertex, end);
            heap.Push(heapVertex);
        }


        public override List<NavVertex> FindPath(List<NavVertex> vertices, NavVertex begin, NavVertex end)
        {
            foreach (var vertex in vertices)
                vertex.ClearState();

            bool pathExists = false;

            var heap = new BinaryHeap<HeapVertex>();
            PushToHeap(heap, begin, end);
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
                        PushToHeap(heap, neighbourVertex, end);
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


    

