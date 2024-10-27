using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;
    public Transform speedometerNeedle;
    public Transform fuelNeedle;

    public float maxMotorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float brakeTorque = 3000f;
    float motor, steering;

    public static float KPH;
    public Text Speed, Fuel;
    public float startingFuel, remainingFuel;

    Rigidbody rb;
    Vector3 movement;
    public bool isReversing, Crashed = false;

    public Text Score;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        remainingFuel = startingFuel;
    }
    void Update()
    {
        UpdateWheels();
        movement = rb.velocity;
        isReversing = Vector3.Dot(transform.forward, movement) < 0;
        UpdateNeedles();
        SetScore();
    }

    void FixedUpdate()
    {
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        if (remainingFuel > 0 && !Crashed)
        {
            rearLeftWheel.motorTorque = motor;
            rearRightWheel.motorTorque = motor;
            frontLeftWheel.steerAngle = steering;
            frontRightWheel.steerAngle = steering;
            if (Input.GetKey(KeyCode.Space) || ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !isReversing) || ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isReversing))
            {
                ApplyBrakes();
            }
            else
            {
                ReleaseBrakes();
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                remainingFuel -= 1 * Time.deltaTime;
            }
        }
        else
        {
            StopCar();
            if (Crashed)
            {
                FindObjectOfType<GameManager>().Crash();
            }
            else
            {
                FindObjectOfType<GameManager>().FuelOut();
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "FuelPickup")
        {
            Crashed = true;
        }
        if (collision.gameObject.tag == "FuelPickup")
        {
            remainingFuel = startingFuel;
            collision.gameObject.SetActive(false);
        }
    }
    void UpdateNeedles()
    {
        KPH = movement.magnitude * 3.6f;
        Speed.text = (int)KPH + " KPH";
        Fuel.text = (int)(remainingFuel / startingFuel * 100) + "%";
        float sNeedleRotation = Mathf.Clamp(KPH / 200f * 250f - 35f, -35f, 215f);
        float fNeedleRotation = Mathf.Clamp(remainingFuel / startingFuel * 250f - 125f, -125f, 125f);
        speedometerNeedle.localEulerAngles = new Vector3(0f, 0f, sNeedleRotation);
        fuelNeedle.localEulerAngles = new Vector3(0f, 0f, fNeedleRotation);
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

    void ApplyBrakes()
    {
        frontLeftWheel.brakeTorque = brakeTorque;
        frontRightWheel.brakeTorque = brakeTorque;
        rearLeftWheel.brakeTorque = brakeTorque;
        rearRightWheel.brakeTorque = brakeTorque;
    }

    void ReleaseBrakes()
    {
        frontLeftWheel.brakeTorque = 0f;
        frontRightWheel.brakeTorque = 0f;
        rearLeftWheel.brakeTorque = 0f;
        rearRightWheel.brakeTorque = 0f;
    }
    public void StopCar()
    {
        rearLeftWheel.motorTorque = 0;
        rearRightWheel.motorTorque = 0;
        rb.velocity = Vector3.zero;
        Invoke("DisableControls", 1f);
    }
    void DisableControls()
    {
        this.enabled = false;
    }
    void SetScore()
    {
        if (remainingFuel / startingFuel > 0.75f)
        {
            Score.text = "Score: A";
        }
        else if (remainingFuel / startingFuel > 0.5f)
        {
            Score.text = "Score: B";
        }
        else if (remainingFuel / startingFuel > 0.25f)
        {
            Score.text = "Score: C";
        }
        else
        {
            Score.text = "Score: D";
        }
    }
}
