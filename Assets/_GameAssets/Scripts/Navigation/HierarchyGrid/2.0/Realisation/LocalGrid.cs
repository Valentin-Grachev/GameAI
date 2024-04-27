using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LocalGrid
{
    private Graph _graph; public Graph graph => _graph;

    private Vector2 _startPosition;
    private float _edgeSize;

    private int _verticesInRow;

    public LocalGrid(Graph graph, Vector2 pivotPosition, int density, float gridSize)
    {
        SetStartParameters(pivotPosition, density, gridSize);
        _graph = graph;
    }


    public LocalGrid(Vector2 startPosition, int density, float size)
    {
        SetStartParameters(startPosition, density, size);
        _graph = GenerateGraph(_verticesInRow);
    }

    private void SetStartParameters(Vector2 startPosition, int density, float gridSize)
    {
        _edgeSize = gridSize / density;
        _startPosition = startPosition + new Vector2(_edgeSize / 2f, -_edgeSize / 2f);
        _verticesInRow = density;
    }


    private Graph GenerateGraph(int size)
    {
        Graph grid = new Graph(size * size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int vertexId = i * size + j;

                if (vertexId % size < size - 1)
                    grid.CreateEdge(vertexId, (ushort)(vertexId + 1), weight: 1);

                if (vertexId / size < size - 1)
                    grid.CreateEdge(vertexId, (ushort)(vertexId + size), weight: 1);
            }
        }

        return grid;
    }

    public Vector2 GetVertexPosition(int vertexId)
    {
        int x = vertexId % _verticesInRow;
        int y = vertexId / _verticesInRow;

        return new Vector2(_startPosition.x + x * _edgeSize, _startPosition.y - y * _edgeSize);
    }


    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;

        for (int vertexId = 0; vertexId < _graph.vertexQuantity; vertexId++)
        {
            Vector2 vertexPosition = GetVertexPosition(vertexId);

            var edges = _graph.GetEdges(vertexId);
            if (edges.Count == 0) continue;

            foreach (var edge in edges)
            {
                Vector2 toVertexPosition = GetVertexPosition(edge.toVertexId);
                Gizmos.DrawLine(vertexPosition, toVertexPosition);
            }

            Gizmos.DrawWireCube(vertexPosition, Vector2.one * _edgeSize / 2f);
        }
    }

    public void RemoveVertex(int vertexId)
    {
        var edges = new List<Edge>(_graph.GetEdges(vertexId));
        foreach (var edge in edges)
            _graph.RemoveEdge(vertexId, edge.toVertexId);
    }


}
