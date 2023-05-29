using UnityEngine;
using VG.GameAI.Navigation2D;

public class Test : MonoBehaviour
{
    [SerializeField] private NavGrid _navGrid;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _navGrid.FindPath(Vector2.zero, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
