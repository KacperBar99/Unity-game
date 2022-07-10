using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class guide : MonoBehaviour
{
    public GameObject controller;
    public TMP_Text prev;
    public TMP_Text next;
    public GameObject prev_button;
    public GameObject next_button;
    public GameObject main_menu;
    public GameObject Guide;
    public int page=0;
    public GameObject[] infos;
    public bool in_game_guide;

    private void Start()
    {
        foreach(GameObject ob in infos)
        {
            ob.SetActive(false);
        }
        page = 0;
        prev_button.SetActive(false);
        next.text = (page + 1).ToString();
        infos[page].SetActive(true);
    }
    

    public void go_back()
    {
        if(controller!=null)
        {
            if(in_game_guide==false)
            controller.GetComponent<menu_controller>().set_menu();
            else
            {
                controller.GetComponent<pause_menu>().set_menu();
            }
        }
        main_menu.SetActive(true);
        Guide.SetActive(false);
    }
    public void next_page()
    {
        infos[page].SetActive(false);
        if (page == 0) prev_button.SetActive(true);
        
        page++;
        prev.text = page.ToString();
        next.text = (1 + page).ToString();
        if(page==infos.Length-1)
        {
            next_button.SetActive(false);
            if (controller != null)
            {
                if (in_game_guide == false)
                    controller.GetComponent<menu_controller>().set_guide();
                else controller.GetComponent<pause_menu>().set_guide();
            }
            
        }
        infos[page].SetActive(true);
    }
    public void last_page()
    {
        infos[page].SetActive(false);
        if (page == infos.Length - 1) next_button.SetActive(true);
        if (page == 1)
        {
            if (controller != null)
            {
                if (in_game_guide == false)
                    controller.GetComponent<menu_controller>().set_guide();
                else controller.GetComponent<pause_menu>().set_guide();
            }
            prev_button.SetActive(false); 
        }
        prev.text = (page-1).ToString();
        next.text = (page).ToString();
        page--;
        infos[page].SetActive(true);
    }
}
