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
    public static List<Vector2> GetBindingNodes(LocalGrid firstGrid, LocalGrid secondGrid, BindSide bindSide)
    {
        var bindingNodes = new List<Vector2>();
        List<Vector2> transitions = new List<Vector2>();
        int verticesInRow = bindSide == BindSide.Right ? firstGrid.rows : firstGrid.cols;

        for (int i = 0; i < verticesInRow; i++)
        {
            // ������ �������
            int firstGridVertexId; 

            if (bindSide == BindSide.Right) // � ������ ����� ������� ������ �������
                firstGridVertexId = firstGrid.cols - 1 + i * firstGrid.cols;

            // � ������ ����� ������� ������ �������
            else firstGridVertexId = firstGrid.rows * (firstGrid.cols - 1) + i;

            Vector2 firstVertexPosition = firstGrid.GetVertexPosition(firstGridVertexId);

            // ������ �������
            int secondGridVertexId;

            if (bindSide == BindSide.Right) // � ������ ����� ������� ����� �������
                secondGridVertexId = i * firstGrid.cols;

            // � ������ ����� ������� ������� �������
            else secondGridVertexId = i;

            Vector2 secondVertexPosition = firstGrid.GetVertexPosition(secondGridVertexId);

            bool hasTransition =
                firstGrid.graph.GetEdges(firstGridVertexId).Count > 0
                && secondGrid.graph.GetEdges(secondGridVertexId).Count > 0;

            // ������� �������� �� �����������
            if (hasTransition) transitions.Add(new Vector2(firstVertexPosition.x,
                (firstVertexPosition.y + secondVertexPosition.y) / 2f));

            // ������� �������� ��������
            else if (transitions.Count > 0)
            {
                // ��������� ����� ���� - �������� �������
                Vector2 middlePosition = (transitions[0] + transitions[transitions.Count - 1]) / 2f;
                bindingNodes.Add(middlePosition);
                transitions.Clear();
            }
        }

        if (transitions.Count > 0)
        {
            // ��������� ����� ���� - �������� �������
            Vector2 middlePosition = (transitions[0] + transitions[transitions.Count - 1]) / 2f;
            bindingNodes.Add(middlePosition);
            transitions.Clear();
        }

        return bindingNodes;
    }



}
