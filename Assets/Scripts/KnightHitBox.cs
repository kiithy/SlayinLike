using UnityEngine;

public class KnightHitBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!");
            other.GetComponent<Orc>().TakeDamage(1);
        }
    }
}
