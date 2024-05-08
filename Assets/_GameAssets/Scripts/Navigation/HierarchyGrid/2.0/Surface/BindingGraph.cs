using System.Collections.Generic;
using UnityEngine;


public class BindingGraph
{
    private List<Node> _nodes; public List<Node> nodes => _nodes;
    private Graph _graph; public Graph graph => _graph; 


    public BindingGraph(List<Vector2> nodePositions, List<EdgeList> vertexEdges)
    {
        _nodes = new List<Node>(nodePositions.Count);
        for (int vertexId = 0; vertexId < nodePositions.Count; vertexId++)
            _nodes.Add(new Node(vertexId, nodePositions[vertexId]));

        _graph = new Graph(vertexEdges.ToArray());
    }

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
            currentNode.pathsBuilt = true;
        }
    }


    private void GetNodeGridPair(Node node, List<LocalGrid> grids, 
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

            int startVertexId = grid.GetVertexId(node.position);
            int finishVertexId = grid.GetVertexId(insideNode.position);

            if (grid.graph.FindPathDijkstra(startVertexId, finishVertexId,
                out var idPath, out int weight))
            {
                var betweenPoints = new List<Vector2>(idPath.Count);
                for (int vertexId = 0; vertexId < idPath.Count; vertexId++)
                    betweenPoints.Add(grid.GetVertexPosition(idPath[vertexId]));

                var path = new Path(betweenPoints.GetSimplifyPath
                    (start: node.position, finish: insideNode.position));

                _graph.CreateEdge(node.vertexId, insideNode.vertexId, weight, path);
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
                edge.path.DrawGizmos(node.position, _nodes[edge.toVertexId].position);

        }


    }


    public enum BindSide { Right, Down }
    public static List<Vector2> GetNodePositions(LocalGrid firstGrid, LocalGrid secondGrid, BindSide bindSide)
    {
        var bindingNodes = new List<Vector2>();
        if (secondGrid.graph.vertexQuantity == 0 || secondGrid.graph.vertexQuantity == 0)
            return bindingNodes;

        List<Vector2> transitions = new List<Vector2>();
        int verticesInRow = bindSide == BindSide.Right ? firstGrid.rows : firstGrid.cols;

        for (int i = 0; i < verticesInRow; i++)
        {
            // Первая вершина
            int firstGridVertexId;

            if (bindSide == BindSide.Right) // У первой сетки берутся правые вершины
                firstGridVertexId = firstGrid.cols - 1 + i * firstGrid.cols;

            // У первой сетки берутся нижние вершины
            else firstGridVertexId = (firstGrid.rows - 1) * firstGrid.cols + i;

            Vector2 firstVertexPosition = firstGrid.GetVertexPosition(firstGridVertexId);

            // Вторая вершина
            int secondGridVertexId;

            if (bindSide == BindSide.Right) // У второй сетки берутся левые вершины
                secondGridVertexId = i * secondGrid.cols;

            // У второй сетки берутся верхние вершины
            else secondGridVertexId = i;

            Vector2 secondVertexPosition = secondGrid.GetVertexPosition(secondGridVertexId);

            bool hasTransition =
                firstGrid.graph.GetEdges(firstGridVertexId).Count > 0
                && secondGrid.graph.GetEdges(secondGridVertexId).Count > 0;

            // Цепочка проходов не прерывается
            if (hasTransition)
                transitions.Add((firstVertexPosition + secondVertexPosition) / 2f);




            // Цепочка проходов прервана
            else if (transitions.Count > 0)
            {
                // Добавляем новый узел - середину прохода
                Vector2 middlePosition = (transitions[0] + transitions[transitions.Count - 1]) / 2f;
                bindingNodes.Add(middlePosition);
                transitions.Clear();
            }
        }

        if (transitions.Count > 0)
        {
            // Добавляем новый узел - середину прохода
            Vector2 middlePosition = (transitions[0] + transitions[transitions.Count - 1]) / 2f;

            bindingNodes.Add(middlePosition);
            transitions.Clear();
        }

        return bindingNodes;
    }


}
