using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LocalGrid
{
    private Graph _graph; public Graph graph => _graph;

    private Vector2 _startPosition, _size;
    private float _edgeSize;

    private int _rows; public int rows => _rows;
    private int _cols; public int cols => _cols;

    public LocalGrid(Graph graph, Vector2 startPosition, Vector2 size, float density)
    {
        SetStartParameters(startPosition, size, density);
        _graph = graph;
    }


    public LocalGrid(Vector2 startPosition, Vector2 size, float density, Collider2D[] obstacles = null)
    {
        SetStartParameters(startPosition, size, density);
        _graph = GenerateGraph(_rows, _cols, obstacles);
    }

    private void SetStartParameters(Vector2 startPosition, Vector2 size, float density)
    {
        Binding.SplitArea(size, density, out _edgeSize, out _rows, out _cols);
        _size = size;
        _startPosition = startPosition + new Vector2(_edgeSize / 2f, -_edgeSize / 2f);
    }


    private Graph GenerateGraph(int rows, int cols, Collider2D[] obstacles)
    {
        Graph grid = new Graph(rows * cols);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int vertexId = i * cols + j;

                if (vertexId % cols < cols - 1)
                    grid.CreateEdge(vertexId, (ushort)(vertexId + 1), weight: 1);

                if (vertexId / cols < rows - 1)
                    grid.CreateEdge(vertexId, (ushort)(vertexId + cols), weight: 1);
            }
        }

        foreach (var obstacle in obstacles)
        {
            for (int vertexId = 0; vertexId < grid.vertexQuantity; vertexId++)
            {
                if (obstacle.OverlapPoint(GetVertexPosition(vertexId)))
                {
                    var edges = new List<Edge>(grid.GetEdges(vertexId));

                    foreach (var edge in edges)
                        grid.RemoveEdge(vertexId, edge.toVertexId);
                }
            }
        }


        return grid;
    }

    public Vector2 GetVertexPosition(int vertexId)
    {
        int x = vertexId % _cols;
        int y = vertexId / _cols;

        return new Vector2(_startPosition.x + x * _edgeSize, _startPosition.y - y * _edgeSize);
    }


    public void DrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector2 drawGridSizePosition = _startPosition + new Vector2(_size.x / 2f, -_size.y / 2f) 
            + new Vector2(-_edgeSize / 2f, _edgeSize / 2f);

        Gizmos.DrawWireCube(drawGridSizePosition, _size);

        Gizmos.color = Color.blue;

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
