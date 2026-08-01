using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage=10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ICanTakeDamage playerHealth = collision.gameObject.GetComponent<ICanTakeDamage>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damage, Vector2.zero, gameObject);
            }
        }
    }
}
