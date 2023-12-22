using UnityEngine;


namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public class GridVertex
    {
        private LocalGrid _localGraph;
        private int _gridId;
        private GridVertex[] _neighbours;


        public Vector2 worldPosition
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }


        public GridVertex(LocalGrid localGraph, int gridId)
        {
            _localGraph = localGraph;
            _gridId = gridId;
        }

        public void Bind(in GridVertex[] neighbours)
        {
            _neighbours = neighbours;
        }

    }
}


