using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSurface : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField][Min(1)] private int _surfaceDensity;
    [SerializeField][Min(1)] private int _localGridDensity;


    private List<LocalGrid> _localGrids;
    private Graph _bindingGraph;

    //private Graph _transitGraph;
    //private int _rows, _cols;




    [Button(nameof(Build))] public void Build()
    {
        CreateLocalGrids();


    }


    [Button(nameof(Delete))] private void Delete()
    {

    }



    private void CreateLocalGrids()
    {
        Vector2 startPosition = (Vector2)transform.position + new Vector2(-_size.x / 2f, _size.y / 2f);
        var splitMode = Binding.SplitArea(_size, _surfaceDensity, 
            out float localGridSize, out int localGridRows, out int localGridCols);

        _localGrids = new List<LocalGrid>();
        for (int i = 0; i < localGridRows; i++)  
        {
            for (int j = 0; j < localGridCols; j++)
            {
                Vector2 localStartPosition = startPosition + new Vector2(j * localGridSize, -i * localGridSize);
                var localGrid = new LocalGrid(localStartPosition, Vector2.one * localGridSize, _localGridDensity);
                _localGrids.Add(localGrid);

                // Добавление еще одной укороченной колонки сеток справа
                if (j == localGridCols - 1 && splitMode == Binding.SplitMode.Vertical)
                {
                    localStartPosition.x += localGridSize;
                    Vector2 limitedGridSize = new Vector2
                        (_size.x - localGridSize * localGridCols, localGridSize);
                    var limitedLocalGrid = new LocalGrid(localStartPosition, limitedGridSize, _localGridDensity);
                    _localGrids.Add(limitedLocalGrid);
                }
            }


            // Добавление еще одной укороченной строки сеток снизу
            if (j == localGridCols - 1 && splitMode == Binding.SplitMode.Horizontal)
            {
                localStartPosition.x += localGridSize;
                Vector2 limitedGridSize = new Vector2
                    (_size.x - localGridSize * localGridCols, localGridSize);
                var limitedLocalGrid = new LocalGrid(localStartPosition, limitedGridSize, _localGridDensity);
                _localGrids.Add(limitedLocalGrid);
            }

        }





    }

    private void CreateBingingGraph()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector2.one * _size);


        if (_localGrids != null)
        {
            foreach (var localGrid in _localGrids)
                localGrid.DrawGizmos();
        }
        


    }


}
