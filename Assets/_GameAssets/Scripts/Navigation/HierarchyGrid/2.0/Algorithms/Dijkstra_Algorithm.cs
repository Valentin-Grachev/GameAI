using System.Collections.Generic;
using System.Linq;

public static class Dijkstra_Algorithm
{
    const ushort infinity = ushort.MaxValue;

    public static bool FindPathDijkstra(this Graph graph, ushort startVertexId,
            ushort finishVertexId, out List<ushort> path)
    {
        path = null;

        // ��� 0:

        // ��������� ������� - ������� �����
        // ��������� - �����������
        ushort[] vertexMarks = Enumerable.Repeat(infinity, graph.vertexQuantity).ToArray();
        vertexMarks[startVertexId] = 0;

        // ������, ������������, ����� �� ������� ���������� �����
        bool[] hasConstMarks = Enumerable.Repeat(false, graph.vertexQuantity).ToArray();

        // ������, ������������ ����������� id �������� ������
        ushort[] enterVertexIds = Enumerable.Repeat(infinity, graph.vertexQuantity).ToArray();


        ushort currentVertexId = startVertexId;
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

        path = new List<ushort>();

        while (currentVertexId != startVertexId)
        {
            path.Add(currentVertexId);
            currentVertexId = enterVertexIds[currentVertexId];
        }

        path.Reverse();
        return true;
    }





}
