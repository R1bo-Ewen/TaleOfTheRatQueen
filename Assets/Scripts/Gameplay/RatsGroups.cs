using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

public class RatsGroups : MonoBehaviour
{
    public int nbOfRats = 1;
    private bool inFoodRange;
    private List<GameObject> ratsList;
    private bool isActiveGroup = true;
    public Player player;
    [SerializeField] private GameObject ratsContainer;
    [SerializeField] private GameObject ratPrefab;
    public STATE state;
    public enum STATE
    {
        IDLE,
        MOVE
    }

    void Start()
    {
        ratsList = new List<GameObject>();
        CreateRat(0f,0f);
        for (int i = 1; i < nbOfRats; i++)
        {
            CreateRat(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
    }

    public void EatFood()
    {
        // Add a rat to the group, and a food to the total of food eaten
        nbOfRats++;
        player.AddRats(1);
        player.EatFood();
        CreateRat(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
    
    public void CreateRat(float posX,float posY)
    {
        // Creation of a new rat, adding him to the list and attributing him, his parents
        GameObject rat = Instantiate(ratPrefab, new Vector3(transform.position.x + Random.Range(0f,1f), 0,transform.position.z + Random.Range(0f,1f)), transform.rotation);
        rat.GetComponent<RatControler>().TransformPlayer = transform;
        rat.GetComponent<RatControler>().randomX = posX;
        rat.GetComponent<RatControler>().randomZ = posY;
        rat.transform.SetParent(ratsContainer.transform);
        ratsList.Add(rat);
    }

    public void TakeDamage(int incomingDamage)
    {
        nbOfRats -= incomingDamage;
        player.AddRats(incomingDamage);
        if (nbOfRats > 0)
        {
            // Killing the rats so the number of rat visible correspond to the number of rats remaining 
            for (int i = 0; i < ratsList.Count - nbOfRats; i++)
            {
                int dyingRat = Random.Range(1, ratsList.Count);
                ratsList[dyingRat].GetComponent<RatControler>().Death();
                ratsList.Remove(ratsList[dyingRat]);
            }
        }
        else
        {
            //End of game
        }
    }

    public void StopMove()
    {
        if (state != STATE.IDLE)
        {
            state = STATE.IDLE;
            foreach (GameObject rat in ratsList)
            {
                rat.GetComponent<RatControler>().Ratnimation.SetTrigger("Idle");
            }
        }
    }
    
    public void Move()
    {
        if (state != STATE.MOVE)
        {
            state = STATE.MOVE;
            foreach (GameObject rat in ratsList)
            {
                rat.GetComponent<RatControler>().Ratnimation.SetTrigger("Move");
            }
        }
    }
}
