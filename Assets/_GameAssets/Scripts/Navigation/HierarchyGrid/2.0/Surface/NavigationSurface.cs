using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSurface : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField][Range(0.1f, 2)] private float _surfaceDensity;
    [SerializeField] [Range(1f, 100f)] private float _localGridDensity;
    [SerializeField] private List<Collider2D> _obstacles;


    private List<LocalGrid> _localGrids;
    private List<Node> _nodes;
    private Graph _bindingGraph;

    private int _localGridRows, _localGridCols;


    [Button(nameof(Build))] public void Build()
    {
        CreateLocalGrids();
        CreateBingingGraph();

    }



    [Button(nameof(Delete))] private void Delete()
    {
        _localGrids = null;
        _nodes = null;
    }


    [Button(nameof(CreateLocalGrids))]
    private void CreateLocalGrids()
    {
        Vector2 startPosition = (Vector2)transform.position + new Vector2(-_size.x / 2f, _size.y / 2f);
        Binding.SplitArea(_size, _surfaceDensity,
            out float localGridSize, out _localGridRows, out _localGridCols);

        _localGrids = new List<LocalGrid>();
        for (int i = 0; i < _localGridRows + 1; i++)
        {
            for (int j = 0; j < _localGridCols + 1; j++)
            {
                Vector2 localStartPosition = startPosition + new Vector2(j * localGridSize, -i * localGridSize);
                Vector2 size = Vector2.one * localGridSize;
                if (i == _localGridRows) size.y = _size.y - localGridSize * _localGridRows;
                if (j == _localGridCols) size.x = _size.x - localGridSize * _localGridCols;

                var localGrid = new LocalGrid(localStartPosition, size,
                    _localGridDensity, _obstacles.ToArray());
                _localGrids.Add(localGrid);
            }
        }

        _localGridRows++;
        _localGridCols++;
    }

    [Button(nameof(CreateBingingGraph))]
    private void CreateBingingGraph()
    {
        var nodePositions = new List<Vector2>();

        for (int i = 0; i < _localGridRows; i++)
        {
            for (int j = 0; j < _localGridCols; j++)
            {
                int localGridId = i * _localGridCols + j;

                if (j != _localGridCols - 1)
                {
                    var nodes = Binding.GetNodePositions(_localGrids[localGridId],
                        _localGrids[localGridId + 1], Binding.BindSide.Right);
                    nodePositions.AddRange(nodes);
                }
                    

                if (i != _localGridRows - 1)
                {
                    var nodes = Binding.GetNodePositions(_localGrids[localGridId],
                        _localGrids[localGridId + _localGridCols], Binding.BindSide.Down);
                    nodePositions.AddRange(nodes);
                }
                    

            }
        }

        _nodes = new List<Node>(capacity: nodePositions.Count);

        for (int vertexId = 0; vertexId < nodePositions.Count; vertexId++)
            _nodes.Add(new Node(vertexId, nodePositions[vertexId]));

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector2.one * _size);


        if (_localGrids != null)
        {
            foreach (var localGrid in _localGrids)
                localGrid.DrawGizmos();
        }
        
        if (_nodes != null)
        {
            Gizmos.color = Color.green;
            foreach (var node in _nodes)
                Gizmos.DrawWireSphere(node.position, 0.2f);
        }


    }


}
