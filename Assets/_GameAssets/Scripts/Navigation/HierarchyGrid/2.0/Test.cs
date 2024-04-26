using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    [SerializeField] private float _edgeSize;
    [SerializeField] private float _gridSize;
    [SerializeField] private List<int> _removeVertexIds;
    [Space(10)]
    [SerializeField] private int _fromVertexId;
    [SerializeField] private int _toVertexId;
    [Space(10)]
    [SerializeField] private GraphData _graphData;

    [SerializeReference] private GridGraph _gridGraph;
    private List<ushort> _path;


    [Button(nameof(Generate))]
    private void Generate()
    {
        _gridGraph = new GridGraph(transform.position, _edgeSize, _gridSize);
        foreach (var item in _removeVertexIds)
            _gridGraph.RemoveVertex(item);
    }

    [Button(nameof(SaveData))]
    private void SaveData()
    {
        GraphData newData = ScriptableObject.CreateInstance<GraphData>();
        newData.graph = new Graph(_gridGraph.graph);

        UnityEditor.AssetDatabase.CreateAsset(newData, "Assets/NewData.asset");
        UnityEditor.AssetDatabase.SaveAssets();
    }

    [Button(nameof(LoadData))]
    private void LoadData()
    {
        _gridGraph = new GridGraph(_graphData, transform.position, _edgeSize, _gridSize);
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
        Gizmos.DrawWireCube(transform.position, Vector2.one * _gridSize);

        _gridGraph?.DrawGizmos(Color.blue);

        if (_path != null)
        {
            Gizmos.color = Color.magenta;



            foreach (var vertexId in _path)
            {
                Vector2 vertexPosition = _gridGraph.GetVertexPosition(vertexId);
                Gizmos.DrawWireSphere(vertexPosition, _edgeSize / 2f);
            }
                

        }
    }

}
