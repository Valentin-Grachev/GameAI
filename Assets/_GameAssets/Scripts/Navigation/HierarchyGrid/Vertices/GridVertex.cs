using UnityEngine;


namespace VG.GameAI.Navigation2D
{

    [System.Serializable]
    public class GridVertex : Vertex
    {
        public int[] neighbourVertexIds;
        public bool isObstacle = false;


        public GridVertex(Vector2 position)
        {
            this.position = position;
            neighbourVertexIds = null;
        }

        public override int[] neighbourIds => neighbourVertexIds;

        public override int id => throw new System.NotImplementedException();

        public void Bind(in int[] neighbourVertexIds)
        {
            this.neighbourVertexIds = neighbourVertexIds;
        }

        public void DrawGizmos(float vertexSize)
        {
            Gizmos.color = isObstacle ? Color.red : Color.blue;
            Gizmos.DrawWireSphere(position, vertexSize / 4f);

            Gizmos.color = Color.magenta;

            if (neighbourVertexIds[0] != -1) Gizmos.DrawLine(position, position + Vector2.up * vertexSize / 2.5f);
            if (neighbourVertexIds[1] != -1) Gizmos.DrawLine(position, position + Vector2.right * vertexSize / 2.5f);
            if (neighbourVertexIds[2] != -1) Gizmos.DrawLine(position, position + Vector2.down * vertexSize / 2.5f);
            if (neighbourVertexIds[3] != -1) Gizmos.DrawLine(position, position + Vector2.left * vertexSize / 2.5f);

        }

        public override float GetEdgeWeight(int neighbourId) => 1f;
    }
}


