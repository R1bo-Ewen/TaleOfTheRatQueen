using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatControler : MonoBehaviour
{
    public Transform TransformPlayer;
    public float randomX;
    public float randomZ;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(TransformPlayer.position.x + randomX, 0, TransformPlayer.position.z +randomZ);
        transform.rotation = TransformPlayer.rotation;
    }
}
