using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CarSelecter : MonoBehaviour
{
    public GameObject Sport, Muscle, Exotic;
    public CinemachineFreeLook FreeLook;
    public Text CarName;
    public Button Prev, Next;
    int Car = 1;
    void Update()
    {
        if (Car == 1 && !Sport.activeSelf)
        {
            SportCar();
        }
        if (Car == 2 && !Muscle.activeSelf)
        {
            MuscleCar();
        }
        if (Car == 3 && !Exotic.activeSelf)
        {
            ExoticCar();
        }
    }
    void SportCar()
    {
        Prev.interactable = false;
        Exotic.SetActive(false);
        Muscle.SetActive(false);
        Sport.SetActive(true);
        FreeLook.LookAt = Sport.transform.Find("LookAt");
        FreeLook.Follow = Sport.transform;
        CarName.text = "SPORT";
    }
    void MuscleCar()
    {
        Prev.interactable = true;
        Next.interactable = true;
        Sport.SetActive(false);
        Exotic.SetActive(false);
        Muscle.SetActive(true);
        FreeLook.LookAt = Muscle.transform.Find("LookAt");
        FreeLook.Follow = Muscle.transform;
        CarName.text = "MUSCLE";
    }
    void ExoticCar()
    {
        Next.interactable = false;
        Sport.SetActive(false);
        Muscle.SetActive(false);
        Exotic.SetActive(true);
        FreeLook.LookAt = Exotic.transform.Find("LookAt");
        FreeLook.Follow = Exotic.transform;
        CarName.text = "EXOTIC";
    }
    public void NextCar()
    {
        Car++;
    }
    public void PrevCar()
    {
        Car--;
    }
    public void Select()
    {
        FindObjectOfType<GameManager>().StartGame();
        if (Car == 1)
        {
            Sport.GetComponent<CarController>().enabled = true;
        }
        else if (Car == 2)
        {
            Muscle.GetComponent<CarController>().enabled = true;
        }
        else
        {
            Exotic.GetComponent<CarController>().enabled = true;
        }
    }

}
