using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PathTest : MonoBehaviour
{
    [SerializeField] private NavigationSurface _navSurface;
    [Space(10)]
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _finishPosition;

    private List<Vector2> _path;


    [Button(nameof(BuildPath))] private void BuildPath()
    {
        _navSurface.BuildPath(_startPosition.position, _finishPosition.position, out _path);
    }


    private void OnDrawGizmos()
    {
        if (_path == null) return;

        foreach (var pathPoint in _path)
        {
            Gizmos.DrawWireSphere(pathPoint, 0.2f);
        }
    }


}
