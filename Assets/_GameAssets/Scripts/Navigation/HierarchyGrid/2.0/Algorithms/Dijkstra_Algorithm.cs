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





}
