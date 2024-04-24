using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public Ennemy enemyPrefab;
    private bool outOfView = true;
    private float timeOOV = 0f;

    public void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.GetComponent<RatsGroups>() != null)
        {
            enemyPrefab.state = Ennemy.STATE.CHASING;
            outOfView = false;
            enemyPrefab.closestEnemy = Col.gameObject;
        }
    }
    public void OnTriggerExit(Collider Col)
    {
        if (Col.gameObject.GetComponent<RatsGroups>() != null)
        {
            outOfView = true;
        }
    }

    public void Update()
    {
        if (enemyPrefab.state == Ennemy.STATE.CHASING && outOfView)
        {
            timeOOV += Time.deltaTime;
            if (10.0f <= timeOOV)
            {
                enemyPrefab.state = Ennemy.STATE.PATROLING;
            }
        }
    }
}
