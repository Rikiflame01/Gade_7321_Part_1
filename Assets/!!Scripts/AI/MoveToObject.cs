using UnityEngine;
using UnityEngine.AI;

public class MoveToObject : MonoBehaviour
{
    public Transform target; // Assign this in the inspector with your empty GameObject
    private NavMeshAgent agent;

    void Start()
    {
        // Get the NavMeshAgent component from the agent
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Update the agent's destination to the target's position every frame
        agent.SetDestination(target.position);
    }
}
