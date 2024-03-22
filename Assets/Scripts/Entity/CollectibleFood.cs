using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CollectibleFood : MonoBehaviour
{
    public int foodEaten;
    private float inFoodRangeTime = 0f;
    private bool inFoodRange;
    private GameObject foodGameObject;
    public void OnTriggerEnter(Collider Col)
    {
        if(Col.gameObject.GetComponent<Food>() != null);
        {
            inFoodRange = true;
            foodGameObject = Col.gameObject;
        }
    }
    
    public void OnTriggerExit(Collider Col)
    {
        if (Col.gameObject.GetComponent<Food>() != null)
        {
            inFoodRange = false;
            inFoodRangeTime = 0f;
            foodGameObject = null;
        }
    }

    void Update()
    {
        if (inFoodRange)
        {
            inFoodRangeTime += Time.deltaTime;
            if (inFoodRangeTime >= foodGameObject.GetComponent<Food>().data.eatingTime)
            {
                foodEaten++;
                inFoodRange = false;
                inFoodRangeTime = 0f;
                Destroy(foodGameObject);
            }
        }
    }
}
