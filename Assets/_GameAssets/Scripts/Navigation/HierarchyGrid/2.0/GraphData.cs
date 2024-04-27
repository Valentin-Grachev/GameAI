using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GraphData", fileName = "GraphData")]
public class GraphData : ScriptableObject
{
    [SerializeField] private EdgeList[] _vertexEdges;

    public Graph GetGraph() => new Graph(_vertexEdges);


    public void SaveGraph(Graph graph)
    {
        UnityEditor.EditorUtility.SetDirty(this);
        _vertexEdges = new EdgeList[graph.vertexQuantity];

        for (int vertexId = 0; vertexId < graph.vertexQuantity; vertexId++)
            _vertexEdges[vertexId].edges = new List<Edge>(graph.GetEdges(vertexId));

        //UnityEditor.EditorUtility.sav
    }


}
