using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elements : MonoBehaviour
{
    public GameObject player;
    public GameObject air;
    public GameObject water;
    public GameObject fire;
    public GameObject ground;

   
    public int selected = -1;
    public GameObject Circle;

    private void Awake()
    {
        air.GetComponent<Image>().color = Color.grey;
        water.GetComponent<Image>().color = Color.blue;
        fire.GetComponent<Image>().color = Color.red;
        ground.GetComponent<Image>().color = Color.yellow;

        



        Color tmp = air.GetComponent<Image>().color;
        air.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
        tmp = ground.GetComponent<Image>().color;
        ground.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
        tmp = fire.GetComponent<Image>().color;
        fire.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
        tmp = water.GetComponent<Image>().color;
        water.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
    }
    private void FixedUpdate()
    {
        selected = player.GetComponent<player_adventure>().power_selected;
        change_selected();
    }
    public void change_selected()
    {
        set_inactive();   

        switch(selected)
        {
            case 0:
                {
                    water.GetComponent<Image>().color = Color.blue;
                    Circle.GetComponent<Image>().color = Color.blue;
                    break;
                }
            case 1:
                {
                    ground.GetComponent<Image>().color = Color.yellow;
                    Circle.GetComponent<Image>().color = Color.yellow;
                    break;
                }
            case 2:
                {
                    fire.GetComponent<Image>().color = Color.red;
                    Circle.GetComponent<Image>().color = Color.red;
                    
                    break;
                }
            case 3:
                {
                    air.GetComponent<Image>().color = Color.gray;
                    Circle.GetComponent<Image>().color = Color.gray;
                    
                    break;
                }
            default:
                {
                    Circle.GetComponent<Image>().color = Color.black;
                    break;
                }
        }
    }
    void set_inactive()
    {
        air.GetComponent<Image>().color = Color.grey;
        water.GetComponent<Image>().color = Color.blue;
        fire.GetComponent<Image>().color = Color.red;
        ground.GetComponent<Image>().color = Color.yellow;





        Color tmp = air.GetComponent<Image>().color;
        air.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
        tmp = ground.GetComponent<Image>().color;
        ground.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
        tmp = fire.GetComponent<Image>().color;
        fire.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
        tmp = water.GetComponent<Image>().color;
        water.GetComponent<Image>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
    }
}
