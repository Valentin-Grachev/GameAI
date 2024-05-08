using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = nameof(SurfaceData), fileName = nameof(SurfaceData))]
public class SurfaceData : ScriptableObject
{
    [SerializeField] private List<Vector2> _nodePositions;
    [SerializeField] private List<EdgeList> _vertexEdges;

    public BindingGraph Load() => new BindingGraph(_nodePositions, _vertexEdges);


    public void Save(BindingGraph bindingGraph)
    {
        _vertexEdges = new List<EdgeList>(bindingGraph.graph.vertexEdges);

        _nodePositions = new List<Vector2>(bindingGraph.nodes.Count);
        for (int i = 0; i < bindingGraph.nodes.Count; i++)
            _nodePositions.Add(bindingGraph.nodes[i].position);
    }


}
