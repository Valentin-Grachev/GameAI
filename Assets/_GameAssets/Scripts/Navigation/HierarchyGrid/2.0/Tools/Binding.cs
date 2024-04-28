using System.Collections.Generic;
using UnityEngine;

public static class Binding
{
    
    public enum SplitMode { Horizontal, Vertical }
    public static SplitMode SplitArea(Vector2 size, int density,
        out float edgeSize, out int rows, out int cols)
    {
        bool horizontalMode = size.x >= size.y;
        bool squareMode = Mathf.Approximately(size.x, size.y);
        edgeSize = Mathf.Max(size.x, size.y) / density;

        if (squareMode)
        {
            rows = density;
            cols = density;
        }
        else
        {
            rows = horizontalMode ? (int)(size.y / edgeSize) : density;
            cols = horizontalMode ? density : (int)(size.x / edgeSize);
        }
        

        return horizontalMode ? SplitMode.Horizontal : SplitMode.Vertical;
    }


    public enum BindSize { Right, Down }
    public static List<Vector2> GetBindingNodes(LocalGrid first, LocalGrid second, BindSize bindSize)
    {
        throw null;
        /*
        var bindingNodePositions = new List<Vector2>();

        // Связывание проходит по всем локальным сеткам
        for (int y = 0; y < _rows; y++)
            for (int x = 0; x < _cols; x++)
            {
                // Связывание по нижнему краю
                if (y != _rows - 1)
                {
                    var downVertices = _localGrids[y * _cols + x].MakeTransitions
                    (_localGrids[(y + 1) * _cols + x], LocalGrid.TransitSide.Down);

                    _transitVertices.AddRange(downVertices);
                }
                // Связывание по правому краю
                if (x != _cols - 1)
                {
                    var rightVertices = _localGrids[y * _cols + x].MakeTransitions
                    (_localGrids[y * _cols + x + 1], LocalGrid.TransitSide.Right);

                    _transitVertices.AddRange(rightVertices);
                }
            }
        */

    }



}
