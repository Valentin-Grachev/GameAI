using UnityEngine;


namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public abstract class Vertex
    {
        public Vector2 position;

        [System.NonSerialized] private Vertex _visitedFrom = null;
        public Vertex visitedFrom => _visitedFrom;


        public abstract int[] neighbourIds { get; }

        public bool visited => _visitedFrom != null;

        public void Visit(Vertex from)
        {
            _visitedFrom = from;
        }

        public void ClearState() => _visitedFrom = null;

    }
}


