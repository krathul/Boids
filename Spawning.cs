using UnityEngine;

public class Spawning:MonoBehaviour
{
    public Boids prefab;
    public int SpawnCount = 10;
    public float SpawnRadius = 10.0f;

    private void Awake()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            Vector3 pos = Random.insideUnitSphere * SpawnRadius;
            Boids boid = Instantiate(prefab);

            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;
            //boid.transform.SetParent(transform);
        }
    }
}
