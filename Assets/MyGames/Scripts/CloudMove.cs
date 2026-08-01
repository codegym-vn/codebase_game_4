using UnityEngine;
[AddComponentMenu("DangSon/CloudMove")]
public class CloudMove : MonoBehaviour
{
    public float speed = 1f;
    public float resetX = -12f;
    public float endX = 12f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if(transform.position.x > endX)
        {
            Vector3 pos = transform.position;
            pos.x = resetX;
            transform.position = pos;
        }
    }
}
