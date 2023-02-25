using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


public class GameStarter : MonoBehaviour
{

    [SerializeField] bool isPaused;
    GameObject pauseMenu;
    [SerializeField] PostProcessVolume volume;
    DepthOfField dof;

    void Awake()
    {
        isPaused = false;
        pauseMenu = GameObject.Find("pauseMenu");
        pauseMenu.SetActive(false);
        volume = FindObjectOfType<PostProcessVolume>();
        dof = volume.profile.GetSetting<DepthOfField>();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("start_menu");
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // continue game
                Time.timeScale = 1;
                isPaused = false;
                pauseMenu.SetActive(false);
                SetBlur(false);
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true;
                pauseMenu.SetActive(true);
                SetBlur(true);
            }
        }

    }

    void SetBlur(bool blur)
    {
        dof.enabled.value = blur;
    }
}
