using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SceneController : MonoBehaviour
{
    public GameObject puasePanel;
    public void LoadGamePlayScene()
	{
        Time.timeScale = 1;
        SceneManager.LoadScene("GamePlay");
	}
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadInstructionsScene()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void onPauseClick()
	{
        puasePanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void onResumeClicked()
    {
        puasePanel.SetActive(false);
        Time.timeScale = 1;

    }

}
