using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System;

public class menu_controller : MonoBehaviour
{

    public GameObject main_first, options_first, scores_first, guide_first;

    public Sprite[] sprites;
    public GameObject main_menu;
    public GameObject scores;
    public GameObject panel;
    public GameObject settings;
    public GameObject multiplayer;
    public GameObject Guide;

    public void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        panel.GetComponent<Image>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        main_menu.SetActive(true);
        scores.SetActive(false);
        settings.SetActive(false);
        multiplayer.SetActive(false);
        Guide.SetActive(false);
        Scene_static_sync.sync_round = 0;
        Scene_static_sync.sync_xp = 0;
        Scene_static_sync.sync_hp = 100;
        Scene_static_sync.bonus = 0;
        
    }
   
    public void set_menu()
    {
        EventSystem.current.SetSelectedGameObject(main_first);
    }
    public void set_options()
    {
        EventSystem.current.SetSelectedGameObject(options_first);
    }
    public void set_scores()
    {
        EventSystem.current.SetSelectedGameObject(scores_first);
    }
    public void set_guide()
    {
        EventSystem.current.SetSelectedGameObject(guide_first);
    }
    public void open_website()
    {
        Application.OpenURL("http://unity3d.com/");
    }
    public void Share()
    {
        
        string timeNow = DateTime.Now.ToString("dd-MMMM-yyyy HHmmss");
        ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory()+"/ScreenShots/ScreenShot " + timeNow + ".png");

        Invoke("open_URL",1f);
    }
    public void open_URL()
    {
        Application.OpenURL("https://www.facebook.com/");
        Application.OpenURL("file://" + Directory.GetCurrentDirectory() + "/ScreenShots");
    }
}
