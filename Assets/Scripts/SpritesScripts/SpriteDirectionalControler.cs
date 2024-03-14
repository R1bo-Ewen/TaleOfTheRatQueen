using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteDirectionn : MonoBehaviour
{
    // Prise de tous les différents angles pour lesquels le sprite devra changer
    [SerializeField] private float backAngle = 36f;
    [SerializeField] private float backThreeAngle = 72f;
    [SerializeField] private float sideAngle = 108f;
    [SerializeField] private float sideThreeAngle = 144f;
    
    // Transform de l'objet (pour savoir où il regarde)
    [SerializeField] private Transform mainTransform;

    [SerializeField] private Animator animator;

    [SerializeField] private SpriteRenderer spriteRenderer;
    // Update is called once per frame
    void Update()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0.0f,Camera.main.transform.forward.z);
        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);
        Vector2 animationDirection = new Vector2(0.01f, -1f);
        float angle = Mathf.Abs(signedAngle);

        if (signedAngle < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        
        if(angle < backAngle)
        {
            Debug.Log("backAngle");
            animationDirection = new Vector2(0.01f, -1f);
        }
        else if(angle < backThreeAngle)
        {
            Debug.Log("backThreeAngle");
            animationDirection = new Vector2(0.75f, -0.75f);
        }
        else if(angle < sideAngle)
        {
            Debug.Log("sideAngle");
            animationDirection = new Vector2(1f, 0f);
        }
        else if(angle < sideThreeAngle)
        {
            Debug.Log("sideThreeAngle");
            animationDirection = new Vector2(0.75f, 0.75f);
        }
        else
        {
            Debug.Log("frontAngle");
            animationDirection = new Vector2(0f, 1f);
        }
        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}
