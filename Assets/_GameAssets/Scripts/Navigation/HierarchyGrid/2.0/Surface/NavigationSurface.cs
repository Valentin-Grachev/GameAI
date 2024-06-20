using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSurface : MonoBehaviour
{
    [SerializeField] private SurfaceData _data;
    [Space(10)]
    [SerializeField] private Vector2 _size;
    [SerializeField][Range(5f, 100f)] private float _localGridSize;
    [SerializeField] [Range(0.1f, 10f)] private float _localGridEdgeSize;
    [Header("Gizmos Settings")]
    [SerializeField] private Color _gridVertexColor;
    [SerializeField] private Color _gridBorderColor;
    [SerializeField] private Color _bindingGraphColor;

    private Vector2 startPosition => 
        (Vector2)transform.position + new Vector2(-_size.x / 2f, _size.y / 2f);

    private List<LocalGrid> _localGrids;
    private BindingGraph _bindingGraph;

    private int _localGridRows, _localGridCols;

    private void Awake() => LoadSurface();



    [Button(nameof(BuildAndSave))] public void BuildAndSave()
    {
        CreateLocalGrids();

        int i = 0;
        foreach (var localGrid in _localGrids)
        {
            localGrid.Generate();
            i++;
        }

        BuildBingingGraph();
        SaveSurface();
    }

    public bool BuildPath(Vector2 start, Vector2 finish, out List<Vector2> path)
    {
        path = new List<Vector2>();
        var startGrid = GetGridByPosition(start);
        var finishGrid = GetGridByPosition(finish);

        var newStartNode = PathFinder.MakeNewNodeInsideGrid(startGrid, _bindingGraph, start);
        var newFinishNode = PathFinder.MakeNewNodeInsideGrid(finishGrid, _bindingGraph, finish);

        bool pathExists = _bindingGraph.graph.FindPathDijkstra(newStartNode.vertexId,
            newFinishNode.vertexId, out var idPath, out int weight);

        if (pathExists) path = PathFinder.ConvertIdPath(_bindingGraph, idPath);

        _bindingGraph.DeleteLastNodes(2);
        return pathExists;
    }


    [Button(nameof(LoadSurface))] private void LoadSurface()
    {
        _size = _data.size;
        transform.position = _data.startPosition + new Vector2(_size.x / 2f, -_size.y / 2f);
        _localGridSize = _data.localGridSize;
        _localGridEdgeSize = _data.localGridEdgeSize;
        _bindingGraph = new BindingGraph(_data.nodePositions, _data.vertexEdges);

        CreateLocalGrids();
    }

    [Button(nameof(SaveSurface))] private void SaveSurface()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_data);
#endif

        _data.startPosition = startPosition;
        _data.size = _size;
        _data.localGridSize = _localGridSize;
        _data.localGridEdgeSize = _localGridEdgeSize;
        _data.vertexEdges = new List<EdgeList>(_bindingGraph.graph.vertexEdges);

        _data.nodePositions = new List<Vector2>(_bindingGraph.nodes.Count);
        for (int i = 0; i < _bindingGraph.nodes.Count; i++)
            _data.nodePositions.Add(_bindingGraph.nodes[i].position);
    }

    [Button(nameof(Delete))] private void Delete()
    {
        _localGrids = null;
        _bindingGraph = null;
    }


    private LocalGrid GetGridByPosition(Vector2 position)
    {
        int gridIndex = PathFinder.GetGridIndexByPosition
            (position, startPosition, _localGridRows, _localGridCols, _localGridSize);
        return _localGrids[gridIndex];
    }
        

    private void CreateLocalGrids()
    {
        _localGridRows = (int)(_size.y / _localGridSize);
        _localGridCols = (int)(_size.x / _localGridSize);

        var obstacles = FindObjectsOfType<Obstacle>();

        bool hasAddictiveColumn = false;
        bool hasAddictiveRow = false;

        _localGrids = new List<LocalGrid>();
        for (int i = 0; i < _localGridRows + 1; i++)
        {
            bool isLastRow = i == _localGridRows;

            for (int j = 0; j < _localGridCols + 1; j++)
            {
                bool isLastColumn = j == _localGridCols;

                Vector2 localStartPosition = startPosition + new Vector2(j * _localGridSize, -i * _localGridSize);
                Vector2 size = Vector2.one * _localGridSize;
                if (i == _localGridRows) size.y = _size.y - _localGridSize * _localGridRows;
                if (j == _localGridCols) size.x = _size.x - _localGridSize * _localGridCols;
                
                var localGrid = new LocalGrid(localStartPosition, size, 
                    _localGridEdgeSize, obstacles);

                if (localGrid.cols > 0 && localGrid.rows > 0)
                {
                    _localGrids.Add(localGrid);

                    if (isLastColumn) hasAddictiveColumn = true;
                    if (isLastRow) hasAddictiveRow = true;
                }
            }
        }

        if (hasAddictiveColumn) _localGridCols++;
        if (hasAddictiveRow) _localGridRows++;
        
    }

    private void BuildBingingGraph()
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
                localGrid.DrawGizmos(_gridVertexColor, _gridBorderColor);
        }

        _bindingGraph?.DrawGizmos(_bindingGraphColor);
    }


}
