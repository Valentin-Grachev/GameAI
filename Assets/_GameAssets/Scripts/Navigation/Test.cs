using UnityEngine;
using VG.GameAI.Navigation2D;

public class Test : MonoBehaviour
{
    [SerializeField] private NavAgent _navAgent;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _navAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
