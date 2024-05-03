using System.Collections.Generic;
using UnityEngine;


public class BindingGraph
{
    private List<Node> _nodes;
    private Graph _graph; public Graph graph => _graph; 


    public BindingGraph(List<Node> nodes, List<LocalGrid> localGrids)
    {
        _nodes = nodes;
        _graph = new Graph(nodes.Count);

        for (int i = 0; i < _nodes.Count; i++)
        {
            var currentNode = _nodes[i];
            GetNodeGridPair(currentNode, localGrids, out var firstGrid, out var secondGrid);

            BindNodeInsideGrid(currentNode, firstGrid);
            BindNodeInsideGrid(currentNode, secondGrid);
            _nodes[i].MarkAsBuilt();
        }
    }


    private void GetNodeGridPair(in Node node, List<LocalGrid> grids, 
        out LocalGrid firstGrid, out LocalGrid secondGrid)
    {
        firstGrid = null; 
        secondGrid = null;

        foreach (var grid in grids)
            if (grid.Overlap(node.position))
            {
                if (firstGrid == null) firstGrid = grid;
                else { secondGrid = grid; break; }
            }
    }


    private void BindNodeInsideGrid(Node node, LocalGrid grid)
    {
        var insideNodes = GetNodesInsideGrid(grid, _nodes);

        foreach (var insideNode in insideNodes)
        {
            if (insideNode.vertexId == node.vertexId || insideNode.pathsBuilt) continue;
            
            Debug.Log($"Bind: {node.vertexId} => {insideNode.vertexId}");

            int startVertexId = grid.GetVertexId(node.position);
            int finishVertexId = grid.GetVertexId(insideNode.position);

            if (grid.graph.FindPathDijkstra(startVertexId, finishVertexId,
                out var idPath, out int weight))
            {
                var betweenPoints = new List<Vector2>(idPath.Count);
                for (int vertexId = 0; vertexId < idPath.Count; vertexId++)
                    betweenPoints.Add(grid.GetVertexPosition(idPath[vertexId]));

                _graph.CreateEdge(node.vertexId, insideNode.vertexId,
                    weight, new Path(betweenPoints));
            }
        }
    }
           

    private List<Node> GetNodesInsideGrid(LocalGrid grid, List<Node> nodes)
    {
        var insideNodePositions = new List<Node>();

        for (int i = 0; i < nodes.Count; i++)
            if (grid.Overlap(nodes[i].position)) 
                insideNodePositions.Add(nodes[i]);

        return insideNodePositions;
    }



    public void DrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var node in _nodes)
        {
            Gizmos.DrawWireSphere(node.position, 0.3f);

            foreach (var edge in _graph.GetEdges(node.vertexId))
            {
                Vector2 from = node.position;
                Vector2 to;
                for (int i = 0; i < edge.path.betweenPoints.Count; i++)
                {
                    to = edge.path.betweenPoints[i];
                    Gizmos.DrawLine(from, to);
                    from = to;
                }
                to = _nodes[edge.toVertexId].position;
                Gizmos.DrawLine(from, to);
            }

        }


    }



}
