using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    [SerializeField] private Vector2 _gridSize;
    [SerializeField] private int _density;
    
    [SerializeField] private List<int> _removeVertexIds;
    [Space(10)]
    [SerializeField] private int _fromVertexId;
    [SerializeField] private int _toVertexId;
    [Space(10)]
    [SerializeField] private GraphData _graphData;

    [SerializeReference] private LocalGrid _gridGraph;
    private List<ushort> _path;


    [Button(nameof(Generate))]
    private void Generate()
    {
        _gridGraph = new LocalGrid(transform.position, _gridSize, _density);
        foreach (var item in _removeVertexIds)
            _gridGraph.RemoveVertex(item);
    }

    [Button(nameof(SaveData))]
    private void SaveData()
    {
        _graphData.SaveGraph(_gridGraph.graph);
    }

    [Button(nameof(LoadData))]
    private void LoadData()
    {
        //_gridGraph = new LocalGrid(_graphData.GetGraph(), transform.position, _density, _gridSize);
    }



    [Button(nameof(Path))]
    private void Path()
    {
        _gridGraph.graph.FindPathDijkstra((ushort)_fromVertexId, (ushort)_toVertexId, out _path);
    }

    [Button(nameof(Clear))]
    private void Clear()
    {
        _path = null;
        _gridGraph = null;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + new Vector2(_gridSize.x / 2f, -_gridSize.y / 2f), 
            Vector2.one * _gridSize);

        _gridGraph?.DrawGizmos();

        if (_path != null)
        {
            Gizmos.color = Color.magenta;



            foreach (var vertexId in _path)
            {
                Vector2 vertexPosition = _gridGraph.GetVertexPosition(vertexId);
                Gizmos.DrawWireSphere(vertexPosition, _density / 2f);
            }
                

        }
    }

}
