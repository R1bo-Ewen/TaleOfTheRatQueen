using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private bool freezeXZAxis = true;
    void LateUpdate()
    {
        if (freezeXZAxis)
        {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
