using System;
using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public abstract class NavAlgorithm : MonoBehaviour
    {

        public abstract List<NavVertex> FindPath(List<NavVertex> vertices, NavVertex begin, NavVertex end);


        protected List<NavVertex> BuildPath(NavVertex end)
        {
            var path = new List<NavVertex>();
            end.state = NavVertex.State.Path;
            NavVertex currentVertex = end;

            while (currentVertex.visitedFrom != null)
            {
                path.Add(currentVertex);
                currentVertex = currentVertex.visitedFrom;
                currentVertex.state = NavVertex.State.Path;
            }

            currentVertex.state = NavVertex.State.Start;
            path.Add(currentVertex);
            path.Reverse();
            return path;
        }

    }
}


    
