using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{
    public bool DoHavepoints;
    private int currentPionts = 0;
    public int PointsNeeds;
    public GameObject LoseGameMenu;
    public GameObject WinGameMenu;
    public GameObject PointsCounter;
    public Text PointsCounterText;
    public Text PointsNeedsText;

    private void Start()
    {
        if (!DoHavepoints)
        {
            PointsCounter.SetActive(false);
            PointsNeeds = 0;
        }
        else
        {
            PointsCounter.SetActive(true);
            PointsNeedsText.text = PointsNeeds.ToString();
        }
    }
    private void Update()
    {
        if (DoHavepoints)
        {
            PointsCounterText.text = currentPionts.ToString();
        }
    }
    private void OnEnable()
    {
        Point.PointsAdd += Addpoint;
        EndPoint.PlayerLeftTheRoom += CheckForWin;
        PlayerMovement.PlayerTouchEnemy += LostGame;
    }

    private void OnDisable()
    {
        Point.PointsAdd -= Addpoint;
        EndPoint.PlayerLeftTheRoom -= CheckForWin;
        PlayerMovement.PlayerTouchEnemy -= LostGame;
    }

    private void CheckForWin()
    {
        if (currentPionts == PointsNeeds)
            WinGame();
        else
            LostGame();
    }

    private void Addpoint() 
    {
        currentPionts++;
    }

    private void LostGame() 
    {
        Time.timeScale = 0;
        LoseGameMenu.SetActive(true);
    }
    private void WinGame()
    {
        Time.timeScale = 0;
        WinGameMenu.SetActive(true);
        PlayerPrefs.SetInt("LevelData", PlayerPrefs.GetInt("LevelData") + 1);
        PlayerPrefs.Save();
    }

    public void RestartScene() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int number)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(number);
    }
}

