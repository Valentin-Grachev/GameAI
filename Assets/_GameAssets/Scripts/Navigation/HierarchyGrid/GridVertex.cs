using UnityEngine;


namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public class GridVertex
    {
        private Vector2 _position;


        private GridVertex[] _neighbours;


        public Vector2 worldPosition
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }


        public GridVertex(Vector2 position)
        {
            _position = position;
            //_gridId = gridId;
        }

        public void Bind(in GridVertex[] neighbours)
        {
            _neighbours = neighbours;
        }

        public void DrawGizmos(float vertexSize)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_position, vertexSize / 4f);
        }


    }
}


