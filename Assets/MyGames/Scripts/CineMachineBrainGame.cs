using Unity.Cinemachine;
using UnityEngine;
[AddComponentMenu("DangSon/CineMachineBrainGame")]
public class CineMachineBrainGame : MonoBehaviour
{
    
    private CinemachineCamera cinemachineCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        cinemachineCamera.Priority = 10;
        cinemachineCamera.enabled = true;
        cinemachineCamera.Target.TrackingTarget = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
