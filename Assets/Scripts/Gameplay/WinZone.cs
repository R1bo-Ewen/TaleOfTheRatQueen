using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public Player player;
    
    public void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.GetComponent<RatsGroups>() != null)
        {
            if (player.FoodEaten >= 4)
            {
                Debug.Log("win !");
                // Win
            }
        }
    }
}
