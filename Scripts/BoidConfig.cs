using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//default properties of a boid

[CreateAssetMenu]
public class BoidConfig:ScriptableObject
{
    public float min_speed = 2.0f;
    public float max_speed = 5.0f;
    public float VisionRadius = 2.5f;
    public float AvoidRadius = 1f;
    
    public float max_steerForce = 3.0f;

    public LayerMask obstable_Mask;
    public float Req_radius = 0.27f;
    public float Obstacle_AvoidDistance = 5f;
}
