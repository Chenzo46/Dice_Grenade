using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barPlacement : MonoBehaviour
{
    [SerializeField] private Transform followPos;

    private void Update()
    {
        transform.position = followPos.position;
    }
}
