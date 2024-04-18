using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;

public class RatsGroups : MonoBehaviour
{
    public int nbOfRats = 1;
    public int FoodEaten = 0;
    private float inFoodRangeTime = 0f;
    private bool inFoodRange;
    private GameObject foodGameObject;
    private List<GameObject> ennemiesInRange;
    private List<GameObject> ratsList;
    public Player player;
    [SerializeField] private GameObject ratsContainer;
    [SerializeField] private GameObject ratPrefab;

    void Start()
    {
        ennemiesInRange = new List<GameObject>();
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
            // Loose Function
        }
    }
}
