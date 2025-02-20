using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Asigna el jugador desde el inspector
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // Persigue al jugador
        }
    }
}