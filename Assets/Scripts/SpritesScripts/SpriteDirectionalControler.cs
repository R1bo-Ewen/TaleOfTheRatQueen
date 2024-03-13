using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteDirectionn : MonoBehaviour
{
    [SerializeField] private Transform mainTransform;

    [SerializeField] private Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;
    // Update is called once per frame
    void Update()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0.0f,Camera.main.transform.forward.z);
        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);
        Vector2 animationDirection = new Vector2(0.0f, -1f);
        //float angle
    }
}
