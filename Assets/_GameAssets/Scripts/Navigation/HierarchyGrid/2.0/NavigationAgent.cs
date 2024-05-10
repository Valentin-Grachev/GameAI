using System.Collections.Generic;
using UnityEngine;

public class NavigationAgent : MonoBehaviour
{
    [SerializeField] private NavigationSurface _surface;
    [SerializeField] private float _speed;

    private List<Vector2> _currentPath;
    private int _currentPointIndex;

    private float epsilon => _speed * 0.1f;


    public void SetDestination(Vector2 position)
    {
        enabled = _surface.BuildPath(transform.position, position, out _currentPath);
        _currentPointIndex = 0;
    }

    private void Update() => HandleMoving();


    private void HandleMoving()
    {
        if (IsNearPosition(_currentPath[_currentPointIndex]))
        {
            _currentPointIndex++;
            if (_currentPointIndex == _currentPath.Count)
            {
                enabled = false;
                return;
            }
        }
            
        Vector2 moveDirection = _currentPath[_currentPointIndex] - (Vector2)transform.position;
        transform.Translate(moveDirection.normalized * _speed * Time.deltaTime);
    }

    private bool IsNearPosition(Vector2 position)
        => Vector2.Distance(transform.position, position) < epsilon;

}
