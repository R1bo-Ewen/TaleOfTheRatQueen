using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteDirection3Side : MonoBehaviour
{
    // Prise de tous les différents angles pour lesquels le sprite devra changer
    [SerializeField] private float backAngle = 65f;
    [SerializeField] private float sideAngle = 155f;
    
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

        if (angle < backAngle)
        {
            animationDirection = new Vector2(0.01f, -1f);
        }
        else if(angle < sideAngle)
        {
            animationDirection = new Vector2(1f, 0f);
        }
        else
        {
            animationDirection = new Vector2(0f, 1f);
        }
        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}
