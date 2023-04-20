using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;


    public static CameraBehaviour instance;

    private void Awake() => instance = this;

    public IEnumerator shake(float time, float magnitude)
    {
        CinemachineBasicMultiChannelPerlin camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camNoise.m_AmplitudeGain = magnitude;
        yield return new WaitForSeconds(time);
        camNoise.m_AmplitudeGain = 0;
    }
    
}
