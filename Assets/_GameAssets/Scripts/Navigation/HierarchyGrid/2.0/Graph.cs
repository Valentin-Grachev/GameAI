using System;
using System.Collections.Generic;


[Serializable]
public class Graph
{
    


    private EdgeList[] _vertexEdges;

    public int vertexQuantity => _vertexEdges.Length;


    public Graph(Graph copy)
    {
        _vertexEdges = new EdgeList[copy.vertexQuantity];
        for (int i = 0; i < vertexQuantity; i++)
            _vertexEdges[i] = new EdgeList(copy._vertexEdges[i]);
    }

    public Graph(EdgeList[] vertexEdges) => _vertexEdges = vertexEdges;


    public Graph(int vertexQuantity)
    {
        if (vertexQuantity > ushort.MaxValue) throw new Exception("Graph size is too large");

        _vertexEdges = new EdgeList[vertexQuantity];

        for (int i = 0; i < vertexQuantity; i++)
            _vertexEdges[i].Initialize();
    }


    public void CreateEdge(int fromVertexId, int toVertexId, int weight)
    {
        _vertexEdges[fromVertexId].edges.Add(new Edge((ushort)toVertexId, weight));
        _vertexEdges[toVertexId].edges.Add(new Edge((ushort)fromVertexId, weight));
    }

    public void CreateEdge(int fromVertexId, int toVertexId, int weight, Path path)
    {
        _vertexEdges[fromVertexId].edges.Add(new Edge((ushort)toVertexId, weight, path));
        _vertexEdges[toVertexId].edges.Add(new Edge((ushort)fromVertexId, weight, path));
    }


    public void RemoveEdge(int fromVertexId, int toVertexId)
    {
        RemoveEdgeElement(fromVertexId, toVertexId);
        RemoveEdgeElement(toVertexId, fromVertexId);
    }

    public List<Edge> GetEdges(int vertexId) => _vertexEdges[vertexId].edges;




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
