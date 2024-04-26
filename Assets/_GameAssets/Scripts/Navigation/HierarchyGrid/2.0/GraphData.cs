using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GraphData", fileName = "GraphData")]
public class GraphData : ScriptableObject
{
    [SerializeField] private List<Edge>[] _vertexEdges;



    public Graph graph;


}
