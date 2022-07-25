using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Generate uniform points on sphere
//https://mathworld.wolfram.com/SpherePointPicking.html
public class SphereUniformDistrib
{
    private readonly int N;
    private readonly float Radius;
    private Vector3[] points;

    public SphereUniformDistrib(int N, float Radius)
    {
        this.N = N;
        this.Radius = Radius;
        this.points = new Vector3[N];
    }

    public Vector3[] Generate()
    {
        for (int i = 0; i < N; i++)
        {
            float t = (float)i / N;
            float phi = 2 * Mathf.PI * (1.0f + Mathf.Sqrt(5)) / 2 * i;

            float theta = Mathf.Acos(1-2*t);
            float x = Radius * Mathf.Sin(theta) * Mathf.Cos(phi);
            float y = Radius * Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = Radius * Mathf.Cos(theta);

            points[i] = new Vector3(x, y, z);
        }
        return points;
    }
}
