using UnityEngine;

public class Coin : MonoBehaviour
{
   public AudioClip AudioClipCollect;
   public int coinValue = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySfxPlayer(AudioClipCollect);
                GameEvent.eventCoin?.Invoke(coinValue);
                Destroy(gameObject);
            }
        }
    }
}
