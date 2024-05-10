using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider2D _collider; public Collider2D Collider => _collider;
}
