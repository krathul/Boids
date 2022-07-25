using UnityEngine;
using System.Collections;

public class Boids : MonoBehaviour
{
    
    //[HideInInspector]
    public Vector3 position;
    //[HideInInspector]
    public Vector3 velocity;
    //[HideInInspector]
    public Vector3 accelaration;
    //[HideInInspector]
    public Vector3 forward;
    //[HideInInspector]
    //public Vector2 FOV;
    //[HideInInspector]
    public int SeenMates;
    //[HideInInspector]
    public Vector3 FlockCenter;
    //[HideInInspector]
    public Vector3 Flockheading;
    //[HideInInspector]
    public Vector3 Avoid;

    public BoidConfig config;

    Transform cachedTransform;

    public void Awake()
    {
        cachedTransform = transform;
    }

    public void Init()
    {
        position = cachedTransform.position;
        velocity = transform.forward * (config.min_speed + config.max_speed) / 2;
        forward = cachedTransform.forward;
    }

    public void UpdateBoid()
    {
        accelaration = Vector3.zero;

        if (SeenMates != 0)
        {
            var AlignmentForce = Steer(Flockheading) * 2f;
            var CohesionForce = Steer(FlockCenter - position) * 1f;
            var SeparationForce = Steer(Avoid) * 2.5f;

            accelaration = AlignmentForce + CohesionForce + SeparationForce;
        }

        if (AvoidObsctacles())
        {
            Debug.Log("Avoid");
            Vector3 CollisionAvoidForce = Steer(FindClearance())*5f;
            accelaration += CollisionAvoidForce;
        }

        velocity += accelaration * Time.deltaTime;
        float speed = Mathf.Clamp(velocity.magnitude, config.min_speed, config.max_speed);
        velocity = velocity.normalized * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = velocity.normalized;

        position = cachedTransform.position;
        forward = velocity.normalized;
    }

    public bool AvoidObsctacles()
    {
        //Debug.Log(cachedTransform.position);
        if(Physics.Raycast(position, forward, 20f))
        Debug.Log("Hit");
        RaycastHit hit;
        if (Physics.SphereCast(position, config.Req_radius, forward, out hit, config.Obstacle_AvoidDistance, config.obstable_Mask))
            return true;
        else { }
        return false;
    }

    public Vector3 FindClearance()
    {
        Vector3[] rayDirection = new SphereUniformDistrib(400, 1).Generate();
        //Debug.Log(rayDirection.GetLength(0));
        foreach (Vector3 dir in rayDirection)
        {
            Vector3 dir_ = cachedTransform.TransformDirection(dir);
            Ray ray = new Ray(position, dir_);
            if (!Physics.SphereCast(ray, config.Req_radius, config.Obstacle_AvoidDistance, config.obstable_Mask))
            {
                return dir_;
            }
        }
        return forward;
    }

    public Vector3 Steer(Vector3 v)
    {
        v = v.normalized * config.max_speed - velocity;
        return Vector3.ClampMagnitude(v, config.max_steerForce);
    }
}

