using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pause_menu : MonoBehaviour
{
    public GameObject pause_Menu;
    public GameObject settings;
    public GameObject guide;

    public GameObject pause_first;
    public GameObject options_first;
    public GameObject guide_first;
    
    private void Start()
    {
        set_menu();
        pause_Menu.SetActive(true);
        settings.SetActive(false);
        guide.SetActive(false);
    }
    public void load_settings()
    {
        set_options();
        settings.SetActive(true);
        pause_Menu.SetActive(false);
    }
    public void load_guide()
    {
        set_guide();
        guide.SetActive(true);
        pause_Menu.SetActive(false);
    }
    public void set_menu()
    {
        EventSystem.current.SetSelectedGameObject(pause_first);
    }
    public void set_options()
    {
        EventSystem.current.SetSelectedGameObject(options_first);
    }
    
    public void set_guide()
    {
        EventSystem.current.SetSelectedGameObject(guide_first);
    }

}
