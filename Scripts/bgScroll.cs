using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    private float offset;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;

        mat.SetTextureOffset("_MainTex", new Vector2(offset,offset));
    }
}
