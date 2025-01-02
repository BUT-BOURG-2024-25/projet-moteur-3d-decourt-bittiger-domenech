using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 5, 10); 

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
