using System;
using System.Collections.Generic;


[Serializable]
public class Graph
{
    private List<EdgeList> _vertexEdges; public List<EdgeList> vertexEdges => _vertexEdges;

    public int vertexQuantity => _vertexEdges.Count;


    public Graph(List<EdgeList> vertexEdges) => _vertexEdges = vertexEdges;


    public Graph(int vertexQuantity)
    {
        if (vertexQuantity > ushort.MaxValue) throw new Exception("Graph size is too large");

        _vertexEdges = new List<EdgeList>(vertexQuantity);

        for (int i = 0; i < vertexQuantity; i++)
            _vertexEdges.Add(new EdgeList(initialize: true));

    }


    public void CreateEdge(int fromVertexId, int toVertexId, int weight)
    {
        _vertexEdges[fromVertexId].edges.Add(new Edge((ushort)toVertexId, weight));
        _vertexEdges[toVertexId].edges.Add(new Edge((ushort)fromVertexId, weight));
    }

    public void CreateEdge(int fromVertexId, int toVertexId, int weight, Path path)
    {
        _vertexEdges[fromVertexId].edges.Add(new Edge((ushort)toVertexId, weight, path));
        _vertexEdges[toVertexId].edges.Add(new Edge((ushort)fromVertexId, weight, path.GetReversed()));
    }


    public void RemoveEdge(int fromVertexId, int toVertexId)
    {
        RemoveEdgeElement(fromVertexId, toVertexId);
        RemoveEdgeElement(toVertexId, fromVertexId);
    }

    public List<Edge> GetEdges(int vertexId) => _vertexEdges[vertexId].edges;

    public int CreateVertex()
    {
        _vertexEdges.Add(new EdgeList(initialize: true));
        return vertexQuantity - 1;
    }
    
    public void DeleteLastVertices(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            int vertexId = vertexQuantity - 1;

            var edges = new List<Edge>(GetEdges(vertexId));
            for (int j = 0; j < edges.Count; j++)
                RemoveEdge(vertexId, edges[j].toVertexId);

            _vertexEdges.RemoveAt(_vertexEdges.Count - 1);
        }
    }
        



    private void RemoveEdgeElement(int firstVertexId, int secondVertexId)
    {
        for (int i = 0; i < _vertexEdges[firstVertexId].edges.Count; i++)
        {
            var edge = _vertexEdges[firstVertexId].edges[i];
            if (edge.toVertexId == secondVertexId)
            {
                _vertexEdges[firstVertexId].edges.RemoveAt(i);
                break;
            }
        }
    }



    



}
