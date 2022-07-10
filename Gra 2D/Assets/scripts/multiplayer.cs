using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiplayer : MonoBehaviour
{
    public GameObject main_menu;
    public GameObject multi;
    public void go_back()
    {
        main_menu.SetActive(true);
        multi.SetActive(false);
    }
}
