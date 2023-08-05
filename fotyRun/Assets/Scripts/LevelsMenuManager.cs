using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenuManager : MonoBehaviour
{
    public void LoadScene(int number) 
    {
        SceneManager.LoadScene(number);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
