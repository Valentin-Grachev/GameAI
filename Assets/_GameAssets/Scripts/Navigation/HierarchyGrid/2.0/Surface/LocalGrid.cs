using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LocalGrid
{
    private Graph _graph; public Graph graph => _graph; 
    public bool generated => _graph != null;

    private Vector2 _startPosition, _size;
    private float _edgeSize;
    private Collider2D[] _obstacles;

    private int _rows; public int rows => _rows;
    private int _cols; public int cols => _cols;


    public LocalGrid(Vector2 startPosition, Vector2 size, float density, Collider2D[] obstacles)
    {
        Utils.SplitArea(size, density, out _edgeSize, out _rows, out _cols);
        _size = size;
        _startPosition = startPosition;
        _obstacles = obstacles;
    }

    public void Generate() => _graph = GenerateGraph(_rows, _cols);


    private Graph GenerateGraph(int rows, int cols)
    {
        Graph grid = new Graph(rows * cols);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int vertexId = i * cols + j;

                if (vertexId % cols < cols - 1)
                    grid.CreateEdge(vertexId, vertexId + 1, weight: 1);

                if (vertexId / cols < rows - 1)
                    grid.CreateEdge(vertexId, vertexId + cols, weight: 1);
            }
        }

        if (_obstacles != null)
        {
            foreach (var obstacle in _obstacles)
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
        }
        


        return grid;
    }

    public Vector2 GetVertexPosition(int vertexId)
    {
        int x = vertexId % _cols;
        int y = vertexId / _cols;

        return new Vector2(_startPosition.x + (0.5f + x) * _edgeSize,
            _startPosition.y - (0.5f + y) * _edgeSize);
    }

    public int GetVertexId(Vector2 position) =>
        PathFinder.GetGridIndexByPosition(position, _startPosition, _rows, _cols, _edgeSize);


    public void DrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector2 drawGridSizePosition = _startPosition 
            + new Vector2(_size.x / 2f, -_size.y / 2f);

        Gizmos.DrawWireCube(drawGridSizePosition, _size);

        if (!generated) return;

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

    public bool Overlap(Vector2 position)
    {
        float eps = 0.1f;
        Vector2 gridSize = new Vector2(_edgeSize * (cols + 1), _edgeSize * (rows + 1));

        float differenceX = position.x - _startPosition.x;
        float differenceY = -position.y + _startPosition.y;

        return -eps <= differenceX && differenceX < gridSize.x + eps 
            && -eps <= differenceY && differenceY < gridSize.y + eps;
    }



}
