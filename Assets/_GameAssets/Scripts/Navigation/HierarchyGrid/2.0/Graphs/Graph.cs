using System;
using System.Collections.Generic;


[Serializable]
public class Graph
{
    [System.Serializable]
    public struct VertexEdges
    {
        public List<Edge> edges;
    }


    private VertexEdges[] _vertexEdges;

    public int vertexQuantity => _vertexEdges.Length;


    public Graph(Graph copy)
    {
        _vertexEdges = new List<Edge>[copy.vertexQuantity];
        for (int i = 0; i < vertexQuantity; i++)
            _vertexEdges[i] = new List<Edge>(copy.GetEdges(i));
    }

    public Graph(int vertexQuantity)
    {
        if (vertexQuantity > ushort.MaxValue) throw new Exception("Graph size is too large");

        _vertexEdges = new List<Edge>[vertexQuantity];

        for (int i = 0; i < vertexQuantity; i++)
            _vertexEdges[i] = new List<Edge>();
    }


    public void CreateEdge(int fromVertexId, int toVertexId, int weight)
    {
        _vertexEdges[fromVertexId].Add(new Edge((ushort)toVertexId, weight));
        _vertexEdges[toVertexId].Add(new Edge((ushort)fromVertexId, weight));
    }

    public void RemoveEdge(int fromVertexId, int toVertexId)
    {
        RemoveEdgeElement(fromVertexId, toVertexId);
        RemoveEdgeElement(toVertexId, fromVertexId);
    }

    public List<Edge> GetEdges(int vertexId) => _vertexEdges[vertexId];




    private void RemoveEdgeElement(int firstVertexId, int secondVertexId)
    {
        for (int i = 0; i < _vertexEdges[firstVertexId].Count; i++)
        {
            var edge = _vertexEdges[firstVertexId][i];
            if (edge.toVertexId == secondVertexId)
            {
                _vertexEdges[firstVertexId].RemoveAt(i);
                break;
            }
        }
    }



    



}
