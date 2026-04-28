using UnityEngine;

public class DestroySelfOnContact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
