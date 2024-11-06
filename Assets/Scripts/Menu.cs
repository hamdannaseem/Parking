using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameManager gm;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void MenuUI()
    {
        gm.Reset();
        SceneManager.LoadScene("Menu");
    }
    public void Level1()
    {
        gm.Reset();
        SceneManager.LoadScene("Level 1");
    }
    public void Level2()
    {
        gm.Reset();
        SceneManager.LoadScene("Level 2");
    }
    public void Level3()
    {
        gm.Reset();
        SceneManager.LoadScene("Level 3");
    }
    public void Level4()
    {
        gm.Reset();
        SceneManager.LoadScene("Level 4");
    }
    public void Level5()
    {
        gm.Reset();
        SceneManager.LoadScene("Level 5");
    }
    public void Level6()
    {
        gm.Reset();
        SceneManager.LoadScene("Level 6");
    }
    public void Restart()
    {
        gm.Restart();
    }
}
