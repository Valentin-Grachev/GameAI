using UnityEngine.AI;
using UnityEngine;

public class AgentTest : MonoBehaviour
{
    [SerializeField] private NavigationAgent _agent;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _agent.SetDestination(mousePosition);
            _navMeshAgent.isStopped = true;
            _navMeshAgent.speed = 0f;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _navMeshAgent.SetDestination(mousePosition);
            _navMeshAgent.isStopped = false;
            _agent.enabled = false;
        }
    }


}
