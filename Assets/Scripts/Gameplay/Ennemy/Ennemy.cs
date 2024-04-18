using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public EnnemyData data;
    public float attackAnimation = 0;
    public STATE state;
    private GameObject closestEnemy = null;
    private bool isInAttackRange;
    
    public enum STATE 
    {
        CALME,
        ALARMED,
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
            closestEnemy = null;
        }
    }
    
    private void Update()
    {
        if (isInAttackRange && state == STATE.ALARMED)
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
}
