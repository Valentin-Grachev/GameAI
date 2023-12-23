using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public class LocalGrid
    {
        private Vector2 _position;
        private float _vertexSize = 1f;
        private float _graphSize = 10f;

        private int verticesInRow => (int)(_graphSize / _vertexSize);



        private List<GridVertex> _vertices;

        private int verticesQuantity => (int)(_graphSize * _graphSize / _vertexSize);



        public LocalGrid(Vector2 position, float vertexSize, float graphSize)
        {
            _position = position;
            _vertexSize = vertexSize;
            _graphSize = graphSize;

            Build();
        }


        public void Build()
        {
            CreateVertices();
            BindVertices();
        }


        private void CreateVertices()
        {
            _vertices = new List<GridVertex>();

            int cols = (int)(_graphSize / _vertexSize);
            int rows = cols;

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    var vertex = new GridVertex(
                    position: new Vector2(
                        x: _position.x + _vertexSize * x + _vertexSize / 2f,
                        y: _position.y - _vertexSize * y - _vertexSize / 2f));

                    _vertices.Add(vertex);
                }

        }

        private void BindVertices()
        {
            for (int i = 0; i < verticesQuantity; i++)
            {
                int x = i % verticesInRow;
                int y = i / verticesInRow;

                TryGetVertex(x, y, out GridVertex vertex);

                var neighbors = new List<GridVertex>(capacity: 4);
                if (TryGetVertex(x + 1, y, out GridVertex neighbor)) neighbors.Add(neighbor);
                if (TryGetVertex(x - 1, y, out neighbor)) neighbors.Add(neighbor);
                if (TryGetVertex(x, y + 1, out neighbor)) neighbors.Add(neighbor);
                if (TryGetVertex(x, y - 1, out neighbor)) neighbors.Add(neighbor);

                vertex.Bind(neighbors.ToArray());
            }
        }



        private bool TryGetVertex(int x, int y, out GridVertex vertex)
        {
            if (x.Between(0, verticesInRow) && y.Between(0, verticesInRow))
            {
                vertex = _vertices[y * verticesInRow + x];
                return true;
            }
            vertex = null;
            return false;
        }


        public void DrawGizmos()
        {
            Vector2 center = new Vector2(
                x: _position.x + _graphSize / 2,
                y: _position.y - _graphSize / 2);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, _graphSize * Vector2.one);

            if (_vertices != null)
                foreach (var vertex in _vertices)
                    vertex.DrawGizmos(_vertexSize);
            
        }



    }
}


