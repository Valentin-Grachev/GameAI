using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTest : MonoBehaviour
{
    [SerializeField] private NavigationAgent _agent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _agent.SetDestination(mousePosition);
        }
    }


}
