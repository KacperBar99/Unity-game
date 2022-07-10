using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;


public class high_scores : MonoBehaviour
{
    public GameObject controller;
    public Sprite[] sprites;
    string[] nickname=new string [10];
    string[] score=new string [10];
    string[] date=new string [10];
    
    public GameObject main_menu;
    public GameObject scores;
    public List<GameObject> buttons1;
    public List<GameObject> buttons2;
    public List<GameObject> buttons3;
    private void Awake()
    {

        
    }
    private void Start()
    {
        int i = 0;


        using (StreamReader reader = new StreamReader(@"score.score"))
        {
            string line1;
            string line2;
            string line3;
            while ((line1 = reader.ReadLine()) != null && i < 10 && line1!="0")
            {
                line2 = reader.ReadLine();
                line3 = reader.ReadLine();
                nickname[i] = line1;
                score[i] = line2;
                date[i] = line3;
                i++;
                
            }
            
        }
        i = 0;
        foreach (GameObject button in buttons1)
        {
            if (i > 2)
            {
                button.GetComponent<Image>().sprite = sprites[3];
            }
            else button.GetComponent<Image>().sprite = sprites[i];
            button.GetComponentInChildren<TMP_Text>().text = nickname[i];
            i++;
        }
        i = 0;
        foreach (GameObject button in buttons2)
        {
            if (i > 2)
            {
                button.GetComponent<Image>().sprite = sprites[3];
            }
            else button.GetComponent<Image>().sprite = sprites[i];
            button.GetComponentInChildren<TMP_Text>().text = score[i];
            i++;
        }
        i = 0;
        foreach (GameObject button in buttons3)
        {
            if (i > 2)
            {
                button.GetComponent<Image>().sprite = sprites[3];
            }
            else button.GetComponent<Image>().sprite = sprites[i];
            button.GetComponentInChildren<TMP_Text>().text = date[i];
            i++;
        }
    }
    public void go_back()
    {
        if(controller!=null)
        controller.GetComponent<menu_controller>().set_menu();
        main_menu.SetActive(true);
        scores.SetActive(false);
    }
}
