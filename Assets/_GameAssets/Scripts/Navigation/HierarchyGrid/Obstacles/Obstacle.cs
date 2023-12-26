using UnityEngine;

namespace VG.GameAI.Navigation2D
{
    public abstract class Obstacle : MonoBehaviour
    {

        public abstract bool IsInside(Vector2 position);
    }
}

