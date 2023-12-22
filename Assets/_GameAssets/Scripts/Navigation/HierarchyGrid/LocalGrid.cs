using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    [System.Serializable]
    public class LocalGrid
    {
        private int _globalId;
        private float _vertexSize = 1f;
        private float _graphSize = 10f;

        private int verticesInRow => (int)(_graphSize / _vertexSize);



        private List<GridVertex> _vertices;

        private int verticesQuantity => (int)(_graphSize * _graphSize / _vertexSize);



        public LocalGrid(int globalId, float vertexSize, float graphSize)
        {
            _globalId = globalId;
            _vertexSize = vertexSize;
            _graphSize = graphSize;
        }


        public void Build()
        {
            CreateVertices();
            BindVertices();
        }


        private void CreateVertices()
        {
            _vertices = new List<GridVertex>();


            for (int i = 0; i < verticesQuantity; i++)
            {
                var vertex = new GridVertex(localGraph: this, i);
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
            Gizmos.color = Color.cyan;
            
        }



    }
}


