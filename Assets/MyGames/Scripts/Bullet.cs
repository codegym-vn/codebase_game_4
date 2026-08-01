using UnityEngine;
[AddComponentMenu("DangSon/Bullet")]
public class Bullet : MonoBehaviour
{
    [Header("FX Prefabs")]
    public GameObject fxPrefabs;
    public int damage = 10;
    [Header("SFX Clips")]
    public AudioClip hitSfx;
    public float lifeTime = 2f;
    private void OnEnable()
    {
        Invoke("DisableBullet", lifeTime);
    }
    void DisableBullet()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(fxPrefabs, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySfxPlayer(hitSfx);
        if(collision.CompareTag("Enemy"))
        {
            ICanTakeDamage enemyHealth = collision.GetComponent<ICanTakeDamage>();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage,Vector2.zero,gameObject);
            }
        }
        gameObject.SetActive(false);
    }
}
