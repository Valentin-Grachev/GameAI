using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public class Rectangle_Obstacle : Obstacle
    {
        private BoxCollider2D _collider;

        public override bool IsInside(Vector2 position)
        {
            _collider = GetComponent<BoxCollider2D>();

            return _collider.bounds.min.x < position.x && position.x < _collider.bounds.max.x &&
                _collider.bounds.min.y < position.y && position.y < _collider.bounds.max.y;
        }
    }
}


