using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    public EnnemyData data;
    public float attackAnimation = 0;
    public STATE state;
    public GameObject closestEnemy = null;
    public List<Vector3> guardePatrol = new List<Vector3>();
    private bool isInAttackRange;
    private int destinationIndex = 0;
    private bool reverseGuardePatrol = false;
    [SerializeField] private NavMeshAgent agent;
    private Vector3 nextDestination;
    
    public enum STATE 
    {
        PATROLING,
        CHASING,
    }
    
    public void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.GetComponent<RatsGroups>() != null)
        {
            isInAttackRange = true;
            closestEnemy = Col.gameObject;
        }
    }
    
    public void OnTriggerExit(Collider Col)
    {
        if (Col.gameObject.GetComponent<RatsGroups>() != null)
        {
            isInAttackRange = false;
            attackAnimation = 0f;
        }
    }

    private void Update()
    {
        if (state == STATE.CHASING)
        {
            agent.destination = closestEnemy.transform.position;
            if (isInAttackRange)
            {
                Vector3 closestEnemyLocation = new Vector3(closestEnemy.transform.position.x,
                    transform.position.y,
                    closestEnemy.transform.position.z);
                transform.LookAt(closestEnemyLocation);
                attackAnimation += Time.deltaTime;
                if (attackAnimation >= data.attackingTime)
                {
                    closestEnemy.GetComponent<RatsGroups>().TakeDamage(data.damage);
                    attackAnimation = 0;
                }
            }
        }

        if (state == STATE.PATROLING)
        {
            if (agent.remainingDistance <= 1f)
            {
                destinationIndex++;
                if (destinationIndex == guardePatrol.Count)
                {
                    destinationIndex = 0;
                }
                agent.destination = guardePatrol[destinationIndex];
            }
        }
    }
}
