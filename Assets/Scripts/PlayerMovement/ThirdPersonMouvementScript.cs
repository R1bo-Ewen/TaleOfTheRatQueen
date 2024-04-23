using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMouvementScript : MonoBehaviour
{
    [SerializeField]
    public CharacterController controller;
    
    [SerializeField]
    public Transform cam;
    
    [SerializeField]
    public float speed = 6f;
    
    [SerializeField]
    public float turnSmoothTime = 0.1f;
    
    [SerializeField]
    private float turnSmoothVelocity;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            GetComponent<RatsGroups>().Move();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            GetComponent<RatsGroups>().StopMove();
        }
    }
}
