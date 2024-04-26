using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridGraph
{
    private Graph _graph; public Graph graph => _graph;

    private Vector2 _startPosition;
    private float _edgeSize;

    private int _verticesInRow;

    public GridGraph(GraphData graphData, Vector2 pivotPosition, float edgeSize, float gridSize)
    {
        SetStartParameters(pivotPosition, edgeSize, gridSize);
        _graph = graphData.graph;
    }


    public GridGraph(Vector2 pivotPosition, float edgeSize, float gridSize)
    {
        SetStartParameters(pivotPosition, edgeSize, gridSize);
        _graph = GenerateGraph(_verticesInRow);
    }

    private void SetStartParameters(Vector2 pivotPosition, float edgeSize, float gridSize)
    {
        _startPosition = new Vector2(pivotPosition.x - gridSize / 2f, pivotPosition.y + gridSize / 2f);
        _startPosition += new Vector2(edgeSize / 2f, -edgeSize / 2f);
        _edgeSize = edgeSize;
        _verticesInRow = (int)(gridSize / edgeSize);
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
