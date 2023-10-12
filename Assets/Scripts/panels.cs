using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class panels : MonoBehaviour
{
    void Awake()
    {
        GetComponent<MeshRenderer>().material.SetTexture("Albedo (RGB) Alpha (A)",GetComponent<P3dPaintableTexture>().Texture);  
    }
    void Update()
    {
        
    }
}
