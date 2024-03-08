using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFood : MonoBehaviour
{
    public int FoodEaten;
    public void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.tag == "Food")
        {
            Debug.Log("Miam miam !");
            FoodEaten++;
            Col.gameObject.SetActive(false);
        }
    }
}
