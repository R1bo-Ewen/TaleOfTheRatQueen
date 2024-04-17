using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int nbOfRats = 1;
    private float inFoodRangeTime = 0f;
    private bool inFoodRange;
    private GameObject foodGameObject;
    private float inEnnemyRangeTime = 0f;
    private bool inEnnemyRange;
    private List<GameObject> ennemiesGameObject;
    [SerializeField] private GameObject ratPrefab;

    void Start()
    {
        for (int i = 0; i < nbOfRats; i++)
        {
            GameObject Rat = Instantiate(ratPrefab, new Vector3(transform.position.x + Random.Range(0f,1f), 0,transform.position.z + Random.Range(0f,1f)), transform.rotation);
            Rat.GetComponent<RatControler>().TransformPlayer = transform;
            if (i == 0)
            {
                Rat.GetComponent<RatControler>().randomX = 0;
                Rat.GetComponent<RatControler>().randomZ = 0;
            }
            else
            {
                Rat.GetComponent<RatControler>().randomX = Random.Range(-1f,1f);
                Rat.GetComponent<RatControler>().randomZ = Random.Range(-1f,1f);
            }
            Rat.transform.SetParent(transform.parent);
        }
    }
    public void OnTriggerEnter(Collider Col)
    {
        if(Col.gameObject.GetComponent<Food>() != null);
        {
            inFoodRange = true;
            foodGameObject = Col.gameObject;
            Col.gameObject.GetComponent<Food>().VFX.SetActive(true);
        }
        if(Col.gameObject.GetComponent<Ennemy>() != null);
        {
            inEnnemyRange = true;
            ennemiesGameObject.Add(Col.gameObject);
        }
    }
    
    public void OnTriggerExit(Collider Col)
    {
        if (Col.gameObject.GetComponent<Food>() != null)
        {
            inFoodRange = false;
            Col.gameObject.GetComponent<Food>().VFX.SetActive(false);
            inFoodRangeTime = 0f;
            foodGameObject = null;
        }
        if (Col.gameObject.GetComponent<Ennemy>() != null)
        {
            Col.gameObject.GetComponent<Ennemy>().attackAnimation = 0f;
            ennemiesGameObject.Remove(Col.gameObject);
        }
    }

    void Update()
    {
        if (inFoodRange)
        {
            inFoodRangeTime += Time.deltaTime;
            if (inFoodRangeTime >= foodGameObject.GetComponent<Food>().data.eatingTime)
            {
                nbOfRats++;
                inFoodRange = false;
                inFoodRangeTime = 0f;
                GameObject Rat = Instantiate(ratPrefab, new Vector3(transform.position.x + Random.Range(0f,1f), 0,transform.position.z + Random.Range(0f,1f)), transform.rotation);
                Rat.GetComponent<RatControler>().TransformPlayer = transform;
                Rat.GetComponent<RatControler>().randomX = Random.Range(-1f,1f);
                Rat.GetComponent<RatControler>().randomZ = Random.Range(-1f,1f);
                Rat.transform.SetParent(transform.parent);
                foodGameObject.GetComponent<Food>().VFX.SetActive(false);
                Destroy(foodGameObject);
            }
        }

        if (ennemiesGameObject.Count != 0)
        {
            foreach (GameObject ennemy in ennemiesGameObject)
            {
                ennemy.GetComponent<Ennemy>().attackAnimation += Time.deltaTime;
                if (ennemy.GetComponent<Ennemy>().attackAnimation >= ennemy.GetComponent<Ennemy>().data.attackingTime)
                {
                    ennemy.GetComponent<Ennemy>().attackAnimation = 0f;
                    nbOfRats -= ennemy.GetComponent<Ennemy>().data.damage;
                }
            }
        }
    }
}
