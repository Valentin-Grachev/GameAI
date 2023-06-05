using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{

    public enum NeighbourSide { Right = 0, Up = 1, Left = 2, Down = 3 }

    [System.Serializable]
    public class NavVertex
    {
        public static bool visualisation;


        public enum State { Empty, Start, Visited, Path, Obstacle }

        public NavGrid navGrid { get; set; }
        public int id { get; private set; }

        private State _state;
        public State state 
        {
            get => _state;
            set
            {
                _state = value;
                VisualiseState();
            }
        }
        public List<int> neighbourIds { get; private set; }
   
        
        public NavVertex visitedFrom;

        private GameObject _instVisualObject;
        

        public NavVertex(NavGrid navGrid, int id, List<int> neighbourIds)
        {
            this.navGrid = navGrid;
            this.id = id;
            this.neighbourIds = neighbourIds;
            state = State.Empty;
        }


        public void GridPosition(out int x, out int y)
        {
            x = id % navGrid.cols;
            y = id / navGrid.cols;
        }

        public Vector2 worldPosition
        {
            get
            {
                GridPosition(out int x, out int y);
                return (Vector2)navGrid.transform.position - navGrid.areaSize / 2f
                    + Vector2.one * navGrid.vertexSize / 2f + new Vector2(x, y) * navGrid.vertexSize;
            }
        }

        public void ClearState()
        {
            if (state != State.Obstacle) state = State.Empty;
            visitedFrom = null;
        }

        public void Visit(NavVertex from)
        {
            visitedFrom = from;
            state = State.Visited;
        }


        public void VisualiseState()
        {
            if (!visualisation) return;

            if (_instVisualObject != null)
                Object.DestroyImmediate(_instVisualObject);

            GameObject visualObject = null;
            switch (state)
            {
                case State.Empty:
                    visualObject = navGrid.data.emptyVertex;
                    break;

                case State.Visited:
                    visualObject = navGrid.data.visitedVertex;
                    break;

                case State.Path:
                    visualObject = navGrid.data.pathVertex;
                    break;

                case State.Start:
                    visualObject = navGrid.data.startVertex;
                    break;

                case State.Obstacle:
                    visualObject = navGrid.data.obstacleVertex;
                    break;
            }

            _instVisualObject = Object.Instantiate(visualObject, worldPosition, Quaternion.identity, navGrid.transform);
            _instVisualObject.transform.localScale *= navGrid.vertexSize;

            if (state == State.Visited || state == State.Path)
            {
                if (visitedFrom == null) return;
                Vector2 lookDirection = (visitedFrom.worldPosition - worldPosition).normalized;
                _instVisualObject.transform.right = lookDirection;
            }

        }


        public static float Distance(NavVertex from, NavVertex to)
        {
            from.GridPosition(out int fromX, out int fromY);
            to.GridPosition(out int toX, out int toY);

            int differentX = fromX - toX;
            int differentY = fromY - toY;

            return Mathf.Sqrt(differentX * differentX + differentY * differentY);
        }


    }
}


