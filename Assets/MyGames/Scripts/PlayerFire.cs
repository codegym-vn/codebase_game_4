using System;
using UnityEngine;
using UnityEngine.InputSystem;
[AddComponentMenu("DangSon/PlayerFire")]
public class PlayerFire : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public float bulletSpeed=30f;
    public Item ammoItem; // Item đại diện cho đạn, dùng để kiểm tra và trừ đạn trong Inventory

    private PlayerController playerController;
    private float afterTime=2f;
  [SerializeField] private int _currentBullets = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        Inventory.Instance.OnInventoryChanged.AddListener(SyncBulletsFomsInventory);
        SyncBulletsFomsInventory(); // Initialize bullet count
    }

    private void SyncBulletsFomsInventory()    
    {
        int pack = Inventory.Instance.GetQuantity(ammoItem);
        if (pack > 0)
        {
            int newBullets = pack * ammoItem.ammoAmount;
            _currentBullets += newBullets;
            Inventory.Instance.UseItem(ammoItem, pack); // Remove the ammo packs from inventory
        }
    }

    // Update is called once per frame
    void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
       */ 
        if (IsFireThisFrame())
        {
            Fire();
        }

    }
    private bool IsFireThisFrame()
    {
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.fKey.wasPressedThisFrame)
                return true;
        }
        var gp = Gamepad.current;
        if (gp != null)
        {
            if (gp.buttonEast.wasPressedThisFrame) // B button
                return true;
        }
        return false;
    }
    private void Fire()
    {
        if(_currentBullets<=0) return;
    
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (playerController.GetFaceRight())
                {
                    rb.linearVelocity = Vector2.right * bulletSpeed;
                }
                else
                {
                    rb.linearVelocity = Vector2.left * bulletSpeed;
                }
            }
            Destroy(bullet, afterTime);
            _currentBullets--;
    }
}
