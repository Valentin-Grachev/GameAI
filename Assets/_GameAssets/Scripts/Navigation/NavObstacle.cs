using UnityEngine;


namespace VG.GameAI.Navigation2D
{
    public class NavObstacle : MonoBehaviour
    {

        public void Place(NavGrid grid)
        {
            Vector2 fromPosition = transform.position - transform.localScale / 2 + Vector3.one * NavGrid.eps;
            Vector2 toPosition = transform.position + transform.localScale / 2 - Vector3.one * NavGrid.eps;

            grid.WorldPositionToGridPosition(fromPosition, onlyInside: false, out int fromX, out int fromY);
            grid.WorldPositionToGridPosition(toPosition, onlyInside: false, out int toX, out int toY);

            print("From: " + fromX + " " + fromY + "; To: " + toX + " " + toY);

            for (int y = fromY; y <= toY; y++)
                for (int x = fromX; x <= toX; x++)
                    SetVertexAsObstacle(grid, x, y);
        }



        private void SetVertexAsObstacle(NavGrid grid, int x, int y)
        {
            NavVertex vertex = grid.data.vertices[grid.GridPositionToId(x, y)];
            vertex.state = NavVertex.State.Obstacle;
            foreach (var neighbourId in vertex.neighbourIds)
            {
                NavVertex neighbourVertex = grid.data.vertices[neighbourId];
                neighbourVertex.neighbourIds.Remove(vertex.id);
            }
            vertex.neighbourIds.Clear();
        }






    }
}


