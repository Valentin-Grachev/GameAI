using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public abstract class HeuristicNavAlgorithm : NavAlgorithm
    {

        private struct HeuristicVertex : IComparable
        {
            public NavVertex navVertex;
            public float heuristic;

            public int CompareTo(object obj)
                => heuristic.CompareTo(((HeuristicVertex)obj).heuristic);
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


        private void PushToHeap(BinaryHeap<HeuristicVertex> heap, NavVertex current, NavVertex begin, NavVertex end)
        {
            var heapVertex = new HeuristicVertex();
            heapVertex.navVertex = current;
            heapVertex.heuristic = GetHeuristic(current, begin, end);
            heap.Push(heapVertex);
        }


        protected abstract float GetHeuristic(NavVertex current, NavVertex begin, NavVertex end);


    }
}




