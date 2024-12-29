using UnityEngine;

public class FlecheBehavior : MonoBehaviour
{
    public delegate void ArrowHitHandler(GameObject arrow, GameObject other);
    public ArrowHitHandler OnArrowHit;

    void OnTriggerEnter(Collider other)
    {
        OnArrowHit?.Invoke(gameObject, other.gameObject);
    }
}
