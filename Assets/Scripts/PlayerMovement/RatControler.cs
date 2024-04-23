using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatControler : MonoBehaviour
{
    public Transform TransformPlayer;
    public float randomX;
    public float randomZ;
    [SerializeField] private ParticleSystem VFXDeath;
    [SerializeField] private GameObject GFX;
    [SerializeField] public Animator Ratnimation;
    private bool isDead = false;
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (!isDead)
        {
            transform.position = new Vector3(TransformPlayer.position.x + randomX, 0, TransformPlayer.position.z +randomZ);
            transform.rotation = TransformPlayer.rotation;
        }
    }
    
    public void Death()
    {
        StartCoroutine(CrtDeath());
    }

    private IEnumerator CrtDeath()
    {
        transform.SetParent(null);
        isDead = true;
        GFX.SetActive(false);
        VFXDeath.Play();
        yield return new WaitForSeconds(VFXDeath.main.duration);
        Destroy(gameObject);
    }
}
