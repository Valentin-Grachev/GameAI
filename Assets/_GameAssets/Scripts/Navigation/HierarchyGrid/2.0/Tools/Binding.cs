using System.Collections.Generic;
using UnityEngine;

public static class Binding
{
    
    public static void SplitArea(Vector2 size, float density,
        out float edgeSize, out int rows, out int cols)
    {
        edgeSize = 10f / density;
        rows = (int)(size.y / edgeSize);
        cols = (int)(size.x / edgeSize);
    }


    public enum BindSide { Right, Down }
    public static List<Vector2> GetNodePositions(LocalGrid firstGrid, LocalGrid secondGrid, BindSide bindSide)
    {
        var bindingNodes = new List<Vector2>();
        if (secondGrid.graph.vertexQuantity == 0 || secondGrid.graph.vertexQuantity == 0) 
            return bindingNodes;

        List<Vector2> transitions = new List<Vector2>();
        int verticesInRow = bindSide == BindSide.Right ? firstGrid.rows : firstGrid.cols;

        Debug.Log($"Vertices in row: {verticesInRow}");

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

            Debug.Log($"First: {firstGridVertexId}, second: {secondGridVertexId}");
            bool hasTransition =
                firstGrid.graph.GetEdges(firstGridVertexId).Count > 0
                && secondGrid.graph.GetEdges(secondGridVertexId).Count > 0;

            // Цепочка проходов не прерывается
            if (hasTransition)
            {
                transitions.Add((firstVertexPosition + secondVertexPosition) / 2f);
                Debug.Log($"first: {firstVertexPosition} second: {secondVertexPosition}");
                Debug.Log((firstVertexPosition + secondVertexPosition) / 2f);
            }
                

            

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
