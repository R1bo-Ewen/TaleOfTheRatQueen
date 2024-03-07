using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFood : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.tag == "Food")
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
