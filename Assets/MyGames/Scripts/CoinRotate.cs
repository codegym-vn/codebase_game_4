using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    public float rotationSpeed = 120f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0, Space.World);
    }
}
