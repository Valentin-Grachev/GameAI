using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _checkPosition;
    [Space(10)]
    [SerializeField] private Vector2 _gridSize;
    [SerializeField] private int _density;

    [SerializeField] private int _fromVertexId;
    [SerializeField] private int _toVertexId;
    [Space(10)]
    [SerializeField] private SurfaceData _graphData;

    [SerializeReference] private LocalGrid _gridGraph;
    private List<int> _path;


    [Button(nameof(Generate))] private void Generate()
    {
        _gridGraph = new LocalGrid(transform.position, _gridSize, _density);
    }

    [Button(nameof(SaveData))] private void SaveData()
    {
        //_graphData.SaveGraph(_gridGraph.graph);
    }

    [Button(nameof(LoadData))] private void LoadData()
    {
        //_gridGraph = new LocalGrid(_graphData.GetGraph(), transform.position, _gridSize, _density);
    }



    [Button(nameof(Path))] private void Path()
    {
        _gridGraph.graph.FindPathDijkstra(_fromVertexId, _toVertexId, out _path, out int weight);
        print(weight);
    }

    [Button(nameof(Clear))] private void Clear()
    {
        _path = null;
        _gridGraph = null;
    }
    [Button(nameof(Check))] private void Check()
    {
        print(_gridGraph.GetVertexId(_checkPosition.transform.position));

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
                Gizmos.DrawWireSphere(vertexPosition, 0.5f);
            }
                

        }
    }

}
