using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public GameObject controller;
   public  GameObject main_menu;
   public  GameObject scores;
    public GameObject settings;
    public GameObject multiplayer;
    public GameObject Guide;
    public void SinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer",LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void load_scores()
    {
        if(controller!=null)
        {
            controller.GetComponent<menu_controller>().set_scores();
        }
        scores.SetActive(true);
        main_menu.SetActive(false);
    }
    public void load_settings()
    {
        if (controller != null)
        {
            controller.GetComponent<menu_controller>().set_options();
        }
        settings.SetActive(true);
        main_menu.SetActive(false);
    }
    public void load_multi()
    {
        multiplayer.SetActive(true);
        main_menu.SetActive(false);
    }
    public void load_guide()
    {
        if (controller != null)
        {
            controller.GetComponent<menu_controller>().set_guide();
        }
        Guide.SetActive(true);
        main_menu.SetActive(false);
    }

}
