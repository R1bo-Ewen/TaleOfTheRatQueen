using UnityEngine;

public class Food : MonoBehaviour
{
     public FoodData data;
     public GameObject VFX;
     private bool isInEatingRange;
     private float eatinState;
     private GameObject closestGroup = null;
     
     public void OnTriggerEnter(Collider Col)
     {
         if (Col.gameObject.GetComponent<RatsGroups>() != null)
         {
             isInEatingRange = true;
             VFX.SetActive(true);
             closestGroup = Col.gameObject;
         }
     }
     
     public void OnTriggerExit(Collider Col)
     {
         if (Col.gameObject.GetComponent<RatsGroups>() != null)
         {
             VFX.SetActive(false);
             isInEatingRange = false;
             closestGroup = null;
         }
     }
     
     private void Update()
     {
         if (isInEatingRange)
         {
             eatinState += Time.deltaTime;
             if (eatinState >= data.eatingTime)
             {
                 closestGroup.GetComponent<RatsGroups>().EatFood();
                 Destroy(gameObject);
             }
         }
     }
}
