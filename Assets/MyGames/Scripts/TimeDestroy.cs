using UnityEngine;

public class TimeDestroy : MonoBehaviour
{
    public float lifetime = 2f; // Lifetime in seconds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,lifetime);
    }
}
