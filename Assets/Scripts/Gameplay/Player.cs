using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int FoodEaten;
    public int totalRats;

    public void EatFood()
    {
        FoodEaten++;
    }

    public void AddRats(int newRats)
    {
        totalRats += newRats;
    }

    public void removeRats(int damages)
    {
        totalRats -= damages;
    }
}
