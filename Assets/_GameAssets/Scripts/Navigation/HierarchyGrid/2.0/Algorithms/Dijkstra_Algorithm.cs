using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Dijkstra_Algorithm
{
    const ushort infinity = ushort.MaxValue;

    public static bool FindPathDijkstra(this Graph graph, int startVertexId,
            int finishVertexId, out List<int> idPath, out int weight)
    {
        idPath = null;
        weight = 0;

        // ��� 0:

        // ��������� ������� - ������� �����
        // ��������� - �����������
        ushort[] vertexMarks = Enumerable.Repeat(infinity, graph.vertexQuantity).ToArray();
        vertexMarks[startVertexId] = 0;

        // ������, ������������, ����� �� ������� ���������� �����
        bool[] hasConstMarks = Enumerable.Repeat(false, graph.vertexQuantity).ToArray();

        // ������, ������������ ����������� id �������� ������
        ushort[] enterVertexIds = Enumerable.Repeat(infinity, graph.vertexQuantity).ToArray();


        ushort currentVertexId = (ushort)startVertexId;
        hasConstMarks[startVertexId] = true;

        // �� ��� ���, ���� �� ��������� �� ������:

        while (currentVertexId != finishVertexId)
        {
            // ��� 1 - ���������� �����
            // ���� �� ������� ����� �� ������� �������

            foreach (var edge in graph.GetEdges(currentVertexId))
            {
                // ���, ���� ������ ���������� ������� ����� �
                // �����, ���������� � ���������� �������� ���� ����� � ����� �������, ������ ���� ������
                ushort currentMark = vertexMarks[edge.toVertexId];
                ushort enterMark = (ushort)(vertexMarks[currentVertexId] + edge.weight);

                if (enterMark < currentMark)
                {
                    vertexMarks[edge.toVertexId] = enterMark;

                    // ������������ �������� �����
                    enterVertexIds[edge.toVertexId] = currentVertexId;
                }
            }

            // ��� 2 - ��������� ���������� �����

            // ����� ������� � ���������� ��������� ������
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

            // ���� ��� ��������� ����� ����� �������������, �� ���� �� ����������
            if (minMark == infinity) return false;

            // ���������� ��������� ����� ���������� ����������
            // � ����������� ������� ��������� ��������
            hasConstMarks[minMarkVertexId] = true;
            currentVertexId = minMarkVertexId;
        }

        // ��� 3 - ���������� ����

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





}
