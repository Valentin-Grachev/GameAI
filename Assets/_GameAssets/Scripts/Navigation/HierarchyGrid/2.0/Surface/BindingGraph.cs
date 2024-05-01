using System.Collections.Generic;
using UnityEngine;


public class BindingGraph : MonoBehaviour
{
    //private List<LocalGrid> _localGrids;
    private List<Node> _nodes;
    private List<Path> _paths;
    private Graph _graph;


    public BindingGraph(List<Node> nodes, List<LocalGrid> localGrids)
    {
        _nodes = nodes;
        _graph = new Graph(nodes.Count);
        CreateBindingGraph(localGrids);
    }




    private void CreateBindingGraph(List<LocalGrid> localGrids)
    {
        foreach (var node in _nodes)
        {
            LocalGrid firstGrid = null, secondGrid = null;

            // ѕоиск пары сеток, которую св€зывает узел
            foreach (var localGrid in localGrids)
            {
                if (localGrid.Overlap(node.position))
                {
                    if (firstGrid == null) firstGrid = localGrid;
                    else
                    {
                        secondGrid = localGrid;
                        break;
                    }
                }
            }

            // ¬ каждой сетке находим другие узлы
            var nodesInsideFirstGrid = GetNodesInsideGrid(firstGrid, _nodes);




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


    private Path GetPath(int fromVertexId, int toVertexId) =>
        _paths[]



}
