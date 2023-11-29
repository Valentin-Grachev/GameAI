using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace VG.GameAI.Navigation2D
{
    public class HeuristicVertex : IComparable
    {
        public NavVertex navVertex;
        public float heuristic;

        public int CompareTo(object obj)
            => heuristic.CompareTo(((HeuristicVertex)obj).heuristic);
    }


    public abstract class HeuristicNavAlgorithm : NavAlgorithm
    {

        

        public override List<NavVertex> FindPath(List<NavVertex> vertices, NavVertex begin, NavVertex end)
        {
            NavMemory.Clear();

            foreach (var vertex in vertices)
                vertex.ClearState();

            bool pathExists = false;

            PushToHeap(begin, begin, end);
            begin.Visit(null);

            while (!NavMemory.isEmpty)
            {
                NavVertex processedVertex = NavMemory.GetMin().navVertex;

                for (int i = 0; i < processedVertex.neighbourIds.Count; i++)
                {
                    NavVertex neighbourVertex = vertices[processedVertex.neighbourIds[i]];

                    if (neighbourVertex.state == NavVertex.State.Empty
                        && neighbourVertex.visitedFrom == null && neighbourVertex.id != begin.id)
                    {
                        neighbourVertex.Visit(from: processedVertex);
                        PushToHeap(neighbourVertex, begin, end);
                        if (neighbourVertex.id == end.id)
                        {
                            pathExists = true;

                            break;
                            
                        }

                        break;
                    }

                    if (i == processedVertex.neighbourIds.Count - 1) NavMemory.PopMin();
                }


                if (pathExists) return BuildPath(end);
            }

            return null;
        }


        private void PushToHeap(NavVertex current, NavVertex begin, NavVertex end)
        {
            var heapVertex = new HeuristicVertex();
            heapVertex.navVertex = current;
            heapVertex.heuristic = GetHeuristic(current, begin, end);
            NavMemory.Add(heapVertex);
        }


        protected abstract float GetHeuristic(NavVertex current, NavVertex begin, NavVertex end);


    }
}




