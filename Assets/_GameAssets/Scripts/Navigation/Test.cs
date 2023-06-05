using UnityEngine;
using VG.GameAI.Navigation2D;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    [SerializeField] private NavAgent _navAgent;
    //[SerializeField] private NavMeshAgent _meshAgent;

    private void Start()
    {
        //_meshAgent.updateRotation = false;
        //_meshAgent.updateUpAxis = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _navAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    
        //    _meshAgent.SetDestination((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    print((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //}
    }
}
