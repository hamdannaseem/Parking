using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera Virtual;
    public CinemachineFreeLook FreeLook;
    public GameObject StartUI, GameUI, EndUI;
    public Text EndText;
    public GameObject Next, Replay;
    public GameObject Diamonds;
    public GameObject[] Diamond = new GameObject[3];
    bool Started = false;
    public static bool Hit = false;
    void Update()
    {
        if (Started)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void StartGame()
    {
        FreeLook.gameObject.SetActive(true);
        Virtual.gameObject.SetActive(false);
        StartUI.SetActive(false);
        GameUI.SetActive(true);
        Started = true;
    }
    public void Crash()
    {
        if (Hit)
        {
            EndText.text = "YOU HIT SOMEONE!";
        }
        else
        {
            EndText.text = "YOU CRASHED!";
        }
        Invoke("Lose", 0.5f);
    }
    public void FuelOut()
    {
        EndText.text = "OUT OF FUEL!";
        Invoke("Lose", 0.5f);
    }
    void Lose()
    {
        EndText.color = new Color(1, 0, 0, 0.5f);
        Next.SetActive(false);
        Replay.SetActive(true);
        GameUI.SetActive(false);
        EndUI.SetActive(true);
        FreeLook.gameObject.SetActive(false);
        Started = false;
    }
    public void Win()
    {
        GameUI.SetActive(false);
        EndUI.SetActive(true);
        FreeLook.gameObject.SetActive(false);
        Started = false;
        Diamonds.SetActive(true);
        int score = (int)(FindObjectOfType<CarController>().remainingFuel / FindObjectOfType<CarController>().startingFuel * 100);
        if (score > 0)
        {
            Diamond[0].SetActive(true);
        }
        if (score > 33)
        {
            Diamond[1].SetActive(true);
        }
        if (score > 66)
        {
            Diamond[2].SetActive(true);
        }
    }
    public void Restart()
    {
        Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Reset()
    {
        ParkingTrigger.Entered = false; ParkingTrigger.Inside = false; ParkingTrigger.Parked = false; Hit = false;
    }
}
