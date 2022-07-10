using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class settings : MonoBehaviour
{
    public AudioMixer audiomixer;
    public GameObject controller;
    public GameObject pause_menu;
    public GameObject main_menu;
    public GameObject setting;
    Resolution[] resolutions;
    public TMP_Dropdown resolution_dropdown;
    public Slider volume_slider;
    public Toggle fullscreen_toggle;
    public Toggle keyboard_toggle;
    

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolution_dropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i=0;i<resolutions.Length;i++)
        {

            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            

            

            
        }
        
            resolution_dropdown.AddOptions(options);

        resolution_dropdown.value = currentResolutionIndex;
        resolution_dropdown.RefreshShownValue();
        
        load_settings();
    }
    public  void go_back()
    {
        if (controller != null) controller.GetComponent<menu_controller>().set_menu();
        save_settings();
        main_menu.SetActive(true);
        setting.SetActive(false);

    }
    public void SetVolume(float volume)
    {
        audiomixer.SetFloat("volume", volume);
    }
    public void SetResolution(int res_index)
    {
        Resolution resolution = resolutions[res_index];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen,resolution.refreshRate);
        Application.targetFrameRate = resolution.refreshRate;
    }
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void go_back_game()
    {
        pause_menu.GetComponent<pause_menu>().set_menu();
        save_settings();
        pause_menu.SetActive(true);
        setting.SetActive(false);
    }
    public void save_settings()
    {
        using (StreamWriter writer = new StreamWriter(@"settings.settings"))
        {
            writer.WriteLine(resolution_dropdown.value);
            writer.WriteLine(volume_slider.value);
            writer.WriteLine(fullscreen_toggle.isOn);
            writer.WriteLine(keyboard_toggle.isOn);
        }
    }
    public void load_settings()
    {
        string line1;
        string line2;
        string line3;
        string line4;
        using (StreamReader reader = new StreamReader(@"settings.settings"))
        {
            line1 = reader.ReadLine();
            line2 = reader.ReadLine();
            line3 = reader.ReadLine();
            line4 = reader.ReadLine();
        }
        if(line1!="-1")
        resolution_dropdown.value = Convert.ToInt32(line1);
        volume_slider.value = Convert.ToSingle(line2);
        fullscreen_toggle.isOn = Convert.ToBoolean(line3);
        keyboard_toggle.isOn = Convert.ToBoolean(line4);
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
