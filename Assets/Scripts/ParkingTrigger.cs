using UnityEngine;
using UnityEngine.UI;

public class ParkingTrigger : MonoBehaviour
{
    public static bool Entered = false, Inside = false, Parked = false;
    public Image Signal;
    public float speed = 0;
    void Start()
    {
        Signal.color = Color.red;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other as MeshCollider != null)
        {
            Entered = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other as MeshCollider != null)
        {
            Entered = false;
            Signal.color = Color.red;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other as MeshCollider != null)
        {
            Inside = IsFullyInsideTrigger(other);
            if (other.GetComponent<Rigidbody>() != null)
            {
                speed = other.GetComponent<Rigidbody>().velocity.magnitude;

                if (Inside)
                {
                    Signal.color = Color.green;
                    if (speed < 0.1)
                    {
                        Parked = true;
                        Invoke("Park", 1.25f);
                    }
                    else
                    {
                        Parked = false;
                    }
                }
                else
                {
                    Parked = false;
                    Signal.color = Color.yellow;
                }
            }
        }
    }
    bool IsFullyInsideTrigger(Collider other)
    {
        Bounds triggerBounds = GetComponent<Collider>().bounds;
        Bounds otherBounds = other.bounds;
        return triggerBounds.min.x <= otherBounds.min.x &&
               triggerBounds.min.y <= otherBounds.min.y &&
               triggerBounds.min.z <= otherBounds.min.z &&
               triggerBounds.max.x >= otherBounds.max.x &&
               triggerBounds.max.y >= otherBounds.max.y &&
               triggerBounds.max.z >= otherBounds.max.z;
    }
    void Park()
    {
        if (Parked)
        {
            FindObjectOfType<CarController>().StopCar();
            FindObjectOfType<GameManager>().Win();
        }
    }
}