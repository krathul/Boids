using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public ComputeShader BoidMaster;
    private ComputeBuffer _Boidbuffer;
    private BoidData[] boidData;
    private int n_Boids;
    private Boids[] boids;
    public BoidConfig boidConfig;

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 forward;
        public int SeenMates;
        public Vector3 FlockCenter;
        public Vector3 Flockheading;
        public Vector3 Avoid;
    }

    private void Awake()
    {
        boids = FindObjectsOfType<Boids>();
        //Debug.Log(boids.GetLength(0));
        n_Boids = boids.GetLength(0);
        boidData = new BoidData[n_Boids];
        //Debug.Log(boidData.GetLength(0));
        for (int i = 0; i < n_Boids; i++)
        {
            boids[i].Init();
        }
    }

    private void SetShaderParams()
    {
        BoidMaster.SetBuffer(0, "boidData", _Boidbuffer);
        BoidMaster.SetInt("n_Boids", n_Boids);
        BoidMaster.SetFloat("AvoidRadius", boidConfig.AvoidRadius);
        BoidMaster.SetFloat("VisionRadius", boidConfig.VisionRadius);
    }

    private void Update()
    {
        if (boids != null)
        {
            for (int i = 0; i < n_Boids; i++)
            {
                boidData[i].position = boids[i].position;
                boidData[i].forward = boids[i].forward;
                boidData[i].FlockCenter = Vector3.zero;
                boidData[i].Flockheading = Vector3.zero;
                boidData[i].Avoid = Vector3.zero;
                boidData[i].SeenMates = 0;
            }

            _Boidbuffer = new ComputeBuffer(n_Boids, 64);
            _Boidbuffer.SetData(boidData);

            SetShaderParams();

            int ThreadGroup = Mathf.CeilToInt(n_Boids / 1024f);
            BoidMaster.Dispatch(0, ThreadGroup, 1, 1);

            _Boidbuffer.GetData(boidData);

            for (int i = 0; i < n_Boids; i++)
            {
                boids[i].FlockCenter = boidData[i].FlockCenter;
                boids[i].Flockheading = boidData[i].Flockheading;
                boids[i].Avoid = boidData[i].Avoid;
                boids[i].SeenMates = boidData[i].SeenMates;

                boids[i].UpdateBoid();
            }
            _Boidbuffer.Release();
        }
    }
}
