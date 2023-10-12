 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using PaintIn3D;
public class PaintLine : MonoBehaviour
{
    private SplineComputer SplineComputer;
    public List<Vector3> points;
    public Color ColorToAdd;
    public GameObject panel;
    public GameObject paintBrush;
    public Texture CurrentPaintingImage;
    void Start()
    {
        paintBrush.SetActive(false);
        SplineComputer = GetComponent<SplineComputer>();
        SplinePoint[] ok = SplineComputer.GetPoints();
        points = new List<Vector3>();
        for (int i = 0; i < ok.Length; i++)
        {
            points.Add(ok[i].position);
        }
        panel.SetActive(false);
        CurrentPaintingImage = panel.GetComponent<P3dPaintableTexture>().Texture;
    }
}
