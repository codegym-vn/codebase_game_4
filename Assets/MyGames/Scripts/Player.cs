using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
[AddComponentMenu("DangSon/Player")]
public class Player : MonoBehaviour, ICanTakeDamage
{
    public int maxhealth = 100;
    private int currentHealth;
    private bool isDead = false;
    private Animator anim;

    private int isDeadId;
    private PlayerController controller;

    [SerializeField] private Item healthSmall;
    [SerializeField] private Item healthLarge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;
        anim = GetComponentInChildren<Animator>();
        isDeadId = Animator.StringToHash("isDead");
        controller = GetComponent<PlayerController>();
    }

  void Die()
    {
        if (isDead) return;
        isDead = true;
        anim.SetTrigger(isDeadId);
        controller.enabled = false;
        Invoke("Respawm", 2f); 

    }
    private void Update()
    {
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.lKey.wasPressedThisFrame)
            {
                UseHealth(healthLarge);
            }
            if (kb.mKey.wasPressedThisFrame)
            {
                UseHealth(healthSmall);
            }
        }

    }
    void Respawm()
    {
      SceneManager.LoadScene("GameOver");
    }
    public void TakeDamage(int damageAmount, Vector2 force, GameObject instigator)
    {
        if (isDead) return;
        currentHealth -= damageAmount;
        GameEvent.eventHealth?.Invoke(currentHealth);
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }

    }
    public bool GetIdead()
    {
        return isDead;
    }
    public void UseHealth(Item healthItem)
    {
        bool used = Inventory.Instance.UseItem(healthItem);
        if (used)
        {
            int before = currentHealth;
            currentHealth = math.min(currentHealth + healthItem.healAmount, maxhealth);  
            GameEvent.eventHealth?.Invoke(currentHealth);
        }
        else
        {
            Debug.Log("No health item available in inventory.");
        }
    }
}
