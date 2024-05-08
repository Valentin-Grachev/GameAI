using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSurface : MonoBehaviour
{
    [SerializeField] private SurfaceData _data;
    [Space(10)]
    [SerializeField] private Vector2 _size;
    [SerializeField][Range(0.1f, 2)] private float _surfaceDensity;
    [SerializeField] [Range(1f, 100f)] private float _localGridDensity;
    [SerializeField] private List<Collider2D> _obstacles;


    private List<LocalGrid> _localGrids;
    private BindingGraph _bindingGraph;

    private int _localGridRows, _localGridCols;


    [Button(nameof(Build))] public void Build()
    {
        CreateLocalGrids();
        CreateBingingGraph();

    }

    [Button(nameof(LoadSurface))] private void LoadSurface()
    {
        _bindingGraph = _data.Load();
    }

    [Button(nameof(SaveSurface))] private void SaveSurface()
    {
        _data.Save(_bindingGraph);
    }

    [Button(nameof(Delete))] private void Delete()
    {
        _localGrids = null;
        _bindingGraph = null;
    }


    [Button(nameof(CreateLocalGrids))]
    private void CreateLocalGrids()
    {
        Vector2 startPosition = (Vector2)transform.position + new Vector2(-_size.x / 2f, _size.y / 2f);
        Utils.SplitArea(_size, _surfaceDensity,
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
                    var addNodePositions = BindingGraph.GetNodePositions(_localGrids[localGridId],
                        _localGrids[localGridId + 1], BindingGraph.BindSide.Right);
                    nodePositions.AddRange(addNodePositions);
                }

                if (i != _localGridRows - 1)
                {
                    var addNodePositions = BindingGraph.GetNodePositions(_localGrids[localGridId],
                        _localGrids[localGridId + _localGridCols], BindingGraph.BindSide.Down);
                    nodePositions.AddRange(addNodePositions);
                }
                    

            }
        }

        var nodes = new List<Node>(capacity: nodePositions.Count);

        for (int vertexId = 0; vertexId < nodePositions.Count; vertexId++)
            nodes.Add(new Node(vertexId, nodePositions[vertexId]));

        _bindingGraph = new BindingGraph(nodes, _localGrids);

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

        _bindingGraph?.DrawGizmos();
    }


}
