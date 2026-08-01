using UnityEngine;
using UnityEngine.UI;

public class UIManagerEnemy : MonoBehaviour
{

    public Slider healthSlide;
    private Enemy enemy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    private void Update()
    {
        healthSlide.value = enemy.GetHealth();
    }
}
