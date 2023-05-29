using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VG.GameAI.Navigation2D
{
    public class NavAgent : MonoBehaviour
    {
        [SerializeField] private NavGrid _navGrid;
        [SerializeField] private float _speed;

        private List<Vector2> _path;
        private int _nextPointIndex;

        private const float eps = 0.1f;

        private bool _goalReached = true;

        public void SetDestination(Vector2 destination)
        {
            _path = _navGrid.FindPath(transform.position, destination);
            _nextPointIndex = 0;
            _goalReached = false;
        }

        private void Update()
        {
            if (_goalReached) return;


            if (Vector2.Distance(transform.position, _path[_nextPointIndex]) < eps)
            {
                if (_nextPointIndex == _path.Count - 1)
                    _goalReached = true;

                else _nextPointIndex++;
            }

            Vector2 moveDirection = _path[_nextPointIndex] - (Vector2)transform.position;
            moveDirection.Normalize();

            transform.Translate(moveDirection * _speed * Time.deltaTime);
        }






    }
}




