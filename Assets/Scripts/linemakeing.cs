using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class linemakeing : MonoBehaviour
{
    private SplineComputer SplineComputer;
    private LineRenderer LineRenderer;
    public List<Vector3> points;
    private void Awake()
    {
        SplineComputer = GetComponent<SplineComputer>();
        LineRenderer = GetComponent<LineRenderer>();
        SplinePoint[] ok = SplineComputer.GetPoints();
        points = new List<Vector3>();
        for (int i = 0; i < ok.Length; i++)
        {
            points.Add(ok[i].position);
        }
        LineRenderer.positionCount = points.Count;
        LineRenderer.SetPositions(points.ToArray());
    }
}
