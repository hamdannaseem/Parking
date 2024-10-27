using System.Collections.Generic;
using UnityEngine;
public class AICarController : MonoBehaviour
{
    Rigidbody rb;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    public float maxMotorTorque;
    public float maxSteeringAngle;

    float Acceleration = 0, TurnAngle = 0, VerticalDir = 0, HorizontalDir = 0, TargetDistance;

    public Transform FirstVertex, RayOrigin;
    Transform CurrentVertex, Target;
    Vector3 TargetPosition, TargetDirection;

    bool Obstacle = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CurrentVertex = FirstVertex;
        Target = CurrentVertex;

    }
    void Update()
    {
        TargetPosition = Target.position;
        TargetDistance = Vector3.Distance(transform.position, TargetPosition);
        MoveCar();
        AvoidObstacles();
        UpdateWheels();
        if (TargetDistance < 1)
        {
            PickNextTarget();
        }
    }
    void FixedUpdate()
    {
        rearLeftWheel.motorTorque = maxMotorTorque * Acceleration;
        rearRightWheel.motorTorque = maxMotorTorque * Acceleration;
        frontLeftWheel.steerAngle = maxSteeringAngle * TurnAngle;
        frontRightWheel.steerAngle = maxSteeringAngle * TurnAngle;
    }
    void PickNextTarget()
    {
        List<Transform> Edges = CurrentVertex.GetComponent<EdgeList>().Edges;
        if (Edges.Count > 0)
        {
            Transform NextVertex = Edges[Random.Range(0, Edges.Count)];
            Target = NextVertex;
            CurrentVertex = NextVertex;
        }
    }
    void MoveCar()
    {
        if (TargetDistance > 1)
        {
            TargetDirection = (TargetPosition - transform.position).normalized;
            VerticalDir = Vector3.Dot(transform.forward, TargetDirection);
            if (!Obstacle)
            {
                Acceleration = 1;
            }
            else
            {
                Acceleration = 0;
            }
            HorizontalDir = Vector3.SignedAngle(transform.forward, TargetDirection, Vector3.up);
            TurnAngle = Mathf.Clamp(HorizontalDir / maxSteeringAngle, -1f, 1f);
        }
        else
        {
            Acceleration = 0;
            TurnAngle = 0;
            StopCar();
        }
    }
    void AvoidObstacles()
    {
        RaycastHit hit;
        bool middleRayHit = Physics.Raycast(RayOrigin.position, RayOrigin.forward, out hit, 5);
        bool leftRayHit = Physics.Raycast(RayOrigin.position + RayOrigin.right * -1, RayOrigin.forward, out hit, 5);
        bool rightRayHit = Physics.Raycast(RayOrigin.position + RayOrigin.right * 1, RayOrigin.forward, out hit, 5);
        bool leftAngleRayHit = Physics.Raycast(RayOrigin.position + RayOrigin.right * -1, Quaternion.Euler(0, -30, 0) * RayOrigin.forward, out hit, 5);
        bool rightAngleRayHit = Physics.Raycast(RayOrigin.position + RayOrigin.right * 1, Quaternion.Euler(0, 30, 0) * RayOrigin.forward, out hit, 5);
        Debug.DrawRay(RayOrigin.position, RayOrigin.forward * 5, Color.green);
        Debug.DrawRay(RayOrigin.position + RayOrigin.right * -1, RayOrigin.forward * 5, Color.green);
        Debug.DrawRay(RayOrigin.position + RayOrigin.right * 1, RayOrigin.forward * 5, Color.green);
        Debug.DrawRay(RayOrigin.position + RayOrigin.right * -1, Quaternion.Euler(0, -30, 0) * RayOrigin.forward * 5, Color.green);
        Debug.DrawRay(RayOrigin.position + RayOrigin.right * 1, Quaternion.Euler(0, 30, 0) * RayOrigin.forward * 5, Color.green);
        if (middleRayHit)
        {
            StopCar();
            Obstacle = true;
            Debug.DrawRay(RayOrigin.position, RayOrigin.forward * 5, Color.red);
        }
        else
        {
            Obstacle = false;
        }
        if (leftRayHit || leftAngleRayHit)
        {
            TurnAngle = 1;
            if (leftRayHit)
            {
                Debug.DrawRay(RayOrigin.position + RayOrigin.right * -1, RayOrigin.forward * 5, Color.red);
            }
            if (leftAngleRayHit)
            {
                Debug.DrawRay(RayOrigin.position + RayOrigin.right * -1, Quaternion.Euler(0, -30, 0) * RayOrigin.forward * 5, Color.green);
            }
        }
        else if (rightRayHit || rightAngleRayHit)
        {
            TurnAngle = -1;
            if (rightRayHit)
            {
                Debug.DrawRay(RayOrigin.position + RayOrigin.right * 1, RayOrigin.forward * 5, Color.red);
            }
            if (rightAngleRayHit)
            {
                Debug.DrawRay(RayOrigin.position + RayOrigin.right * 1, Quaternion.Euler(0, 30, 0) * RayOrigin.forward * 5, Color.red);
            }
        }
    }
    void UpdateWheels()
    {
        UpdateWheelTransform(frontLeftWheel, frontLeftTransform);
        UpdateWheelTransform(frontRightWheel, frontRightTransform);
        UpdateWheelTransform(rearLeftWheel, rearLeftTransform);
        UpdateWheelTransform(rearRightWheel, rearRightTransform);
    }
    void UpdateWheelTransform(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        transform.position = position;
        transform.rotation = rotation;
    }
    void StopCar()
    {
        rearLeftWheel.motorTorque = 0;
        rearRightWheel.motorTorque = 0;
        rb.velocity = Vector3.zero;
    }
}
