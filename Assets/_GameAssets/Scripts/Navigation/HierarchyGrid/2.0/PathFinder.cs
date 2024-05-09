using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class PathFinder
{
    const ushort infinity = ushort.MaxValue;

    public static bool FindPathDijkstra(this Graph graph, int startVertexId,
            int finishVertexId, out List<int> idPath, out int weight)
    {
        idPath = null;
        weight = 0;

        // Шаг 0:

        // Стартовой вершине - нулевая метка
        // Остальным - бесконечная
        ushort[] vertexMarks = Enumerable.Repeat(infinity, graph.vertexQuantity).ToArray();
        vertexMarks[startVertexId] = 0;

        // Массив, показывающий, имеет ли вершина постоянную метку
        bool[] hasConstMarks = Enumerable.Repeat(false, graph.vertexQuantity).ToArray();

        // Массив, запоминающий оптимальные id входящих вершин
        ushort[] enterVertexIds = Enumerable.Repeat(infinity, graph.vertexQuantity).ToArray();


        ushort currentVertexId = (ushort)startVertexId;
        hasConstMarks[startVertexId] = true;

        // До тех пор, пока не доберемся до финиша:

        while (currentVertexId != finishVertexId)
        {
            // Шаг 1 - Обновление меток
            // Идем по каждому ребру из текущей вершины

            foreach (var edge in graph.GetEdges(currentVertexId))
            {
                // Там, куда пришли сравниваем текущую метку и
                // метку, полученную в результате сложения веса ребра и метки вершины, откуда сюда пришли
                ushort currentMark = vertexMarks[edge.toVertexId];
                ushort enterMark = (ushort)(vertexMarks[currentVertexId] + edge.weight);

                if (enterMark < currentMark)
                {
                    vertexMarks[edge.toVertexId] = enterMark;

                    // Запоминается входящее ребро
                    enterVertexIds[edge.toVertexId] = currentVertexId;
                }
            }

            // Шаг 2 - Получение постоянной метки

            // Поиск вершины с наименьшей временной меткой
            ushort minMarkVertexId = infinity;
            ushort minMark = infinity;

            for (ushort vertexId = 0; vertexId < vertexMarks.Length; vertexId++)
            {
                if (!hasConstMarks[vertexId] && vertexMarks[vertexId] < minMark)
                {
                    minMark = vertexMarks[vertexId];
                    minMarkVertexId = vertexId;
                }
            }

            // Если все временные метки равны бесконечности, то пути не существует
            if (minMark == infinity) return false;

            // Наименьшая временная метка становится постоянной
            // и объявляется началом следующей итерации
            hasConstMarks[minMarkVertexId] = true;
            currentVertexId = minMarkVertexId;
        }

        // Шаг 3 - построение пути

        idPath = new List<int>();

        weight = vertexMarks[currentVertexId];
        while (currentVertexId != startVertexId)
        {
            idPath.Add(currentVertexId);
            currentVertexId = enterVertexIds[currentVertexId];
        }

        idPath.Add(startVertexId);
        idPath.Reverse();

        return true;
    }

    public static List<Vector2> GetSimplePath(this List<Vector2> path, Vector2 start, Vector2 finish)
    {
        var newBetweenPoints = new List<Vector2>();
        if (path.Count == 0) return newBetweenPoints;

        Vector2 currentPoint = start;
        Vector2 previousPoint = path[0];
        for (int i = 1; i <= path.Count; i++)
        {
            Vector2 nextPoint = i == path.Count ? finish : path[i];

            Vector2 raycastDirection = (nextPoint - currentPoint).normalized;
            bool hasIntersection = Physics2D.Raycast(currentPoint, raycastDirection, 
                Vector2.Distance(nextPoint, currentPoint)).collider != null;

            if (hasIntersection)
            {
                newBetweenPoints.Add(previousPoint);
                currentPoint = previousPoint;
            }
            previousPoint = nextPoint;
        }

        return newBetweenPoints;
    }

    public static int GetGridIndexByPosition(Vector2 position, Vector2 startPosition, 
        int rows, int cols, float edgeSize)
    {
        int x = (int)((position.x - startPosition.x) / edgeSize);
        x = Mathf.Clamp(x, 0, cols - 1);

        int y = (int)((startPosition.y - position.y) / edgeSize);
        y = Mathf.Clamp(y, 0, rows - 1);

        return y * cols + x;
    }

    public static Path ConvertIdPath(LocalGrid grid, List<int> idPath, Vector2 start, Vector2 finish)
    {
        var betweenPoints = new List<Vector2>(idPath.Count);
        for (int vertexId = 0; vertexId < idPath.Count; vertexId++)
            betweenPoints.Add(grid.GetVertexPosition(idPath[vertexId]));

        return new Path(betweenPoints.GetSimplePath(start, finish));
    }

    public static List<Vector2> ConvertIdPath(BindingGraph bindingGraph, List<int> idPath)
    {
        var pathPoints = new List<Vector2>(idPath.Count);
        for (int i = 1; i < idPath.Count; i++)
        {
            int toVertexId = idPath[i];
            var previousVertexEdges = bindingGraph.graph.GetEdges(idPath[i - 1]);

            for (int k = 0; k < previousVertexEdges.Count; k++)
            {
                if (previousVertexEdges[k].toVertexId == toVertexId)
                {
                    foreach (var pathPoint in previousVertexEdges[k].path.betweenPoints)
                        pathPoints.Add(pathPoint);
                }  
            }
            pathPoints.Add(bindingGraph.nodes[toVertexId].position);
        }

        return pathPoints;
    }


    public static Node MakeNewNodeInsideGrid(LocalGrid grid, 
        BindingGraph bindingGraph, Vector2 newNodePosition)
    {
        if (!grid.generated) grid.Generate();

        var nodes = new List<Node>();
        for (int i = 0; i < bindingGraph.nodes.Count; i++)
            if (grid.Overlap(bindingGraph.nodes[i].position)) 
                nodes.Add(bindingGraph.nodes[i]);

        var newNode = bindingGraph.CreateNode(newNodePosition);
        int startGridVertexId = grid.GetVertexId(newNodePosition);

        foreach (var node in nodes)
        {
            int finishGridVertexId = grid.GetVertexId(node.position);

            if (grid.graph.FindPathDijkstra(startGridVertexId, 
                finishGridVertexId, out var idPath, out int weight))
            {
                var path = ConvertIdPath(grid, idPath, newNodePosition, node.position);
                bindingGraph.graph.CreateEdge(newNode.vertexId, node.vertexId, weight, path);
            }
        }

        return newNode;
    }


}
