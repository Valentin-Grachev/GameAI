using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSurface : MonoBehaviour
{
    [SerializeField] private float _size;
    [SerializeField][Min(1)] private int _localGridRows;
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
        float localGridSize = _size / _localGridRows;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(-_size / 2f, _size / 2f);

        _localGrids = new List<LocalGrid>();
        for (int i = 0; i < _localGridRows; i++)
        {
            for (int j = 0; j < _localGridRows; j++)
            {
                Vector2 localStartPosition = startPosition + new Vector2(j * localGridSize, -i * localGridSize);
                var localGrid = new LocalGrid(localStartPosition, _localGridDensity, localGridSize);
                _localGrids.Add(localGrid);
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
                localGrid.DrawGizmos(Color.blue);
        }
        


    }


}
