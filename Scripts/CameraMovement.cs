using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    [SerializeField] private Vector3 offset;

    public static CameraMovement instance;

    private Vector3 mov;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref mov, smoothTime);
        }
    }

    public void changeTarget(Transform tg)
    {
        target = tg;
    }
}
