using UnityEngine;
[AddComponentMenu("DangSon/BackGroundController")]
public class BackGroundController : MonoBehaviour
{
    private Renderer[] backGrounds;
    public float speedBackGround = 0.1f;
    public float speedMidGround = 0.3f;
    public float speedForeGround = 0.5f;
    float startPosX;
    private Transform Target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGrounds = GetComponentsInChildren<Renderer>();
        if(Target==null )
            Target = GameObject.FindGameObjectWithTag("Vitual Camera").transform;

        startPosX = Target.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        var x = Target.position.x - startPosX;
        if(backGrounds!=null)
        {
            var offsetBack = (x * speedBackGround)%1;
            backGrounds[0].material.mainTextureOffset = new Vector2(offsetBack, backGrounds[0].material.mainTextureOffset.y);
        }
        
    }
}
