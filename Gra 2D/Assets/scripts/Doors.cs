using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Doors : MonoBehaviour
{
    public GameObject water_effect;
    public GameObject ground_effect;
    public GameObject fire_effect;
    public GameObject air_effect;


    public SpriteRenderer spriteRenderer;
    public Sprite active;
    public Sprite inactive;
    public float time_to_inactive = 10f;
    public GameObject teleport;
    public bool cover = false;
    public int cover_type = -1;
    

    public bool is_active = false;

    private void Start()
    {
        is_active = false;
        teleport.GetComponent<BoxCollider2D>().enabled = false;
        spriteRenderer.sprite = inactive;
        
    }
    
    public void change_active()
    {
        is_active = !is_active;
        if (is_active == true)
        {
            teleport.GetComponent<BoxCollider2D>().enabled = true;
            spriteRenderer.sprite = active;
            
        }
        else
        {
            spriteRenderer.sprite = inactive;
            teleport.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void set_teleport(int D)
    {
        teleport.GetComponent<teleport>().direction = D;
    }
    public void activate()
    {
        if (cover == true) return;
        if (is_active == false) change_active();
    }
    public void set_cover(int type)
    {
        cover_type = type;
        switch(cover_type)
        {
            case 0:
                water_effect.SetActive(true);
                cover = true;
                break;
            case 1:
                ground_effect.SetActive(true);
                cover = true;
                break;
            case 2:
                fire_effect.SetActive(true);
                cover = true;
                break;
            case 3:
                air_effect.SetActive(true);
                cover = true;
                break;
                
        }
    }
    public void unset_cover()
    {
        switch(cover_type)
        {
            case 0:
                water_effect.SetActive(false);
                cover = false;
                break;
            case 1:
                ground_effect.SetActive(false);
                cover = false;
                break;
            case 2:
                fire_effect.SetActive(false);
                cover = false;
                break;
            case 3:
                air_effect.SetActive(false);
                cover = false;
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<player_adventure>() != null && collision.gameObject.GetComponent<player_adventure>().power_selected == 3)
        {
            if (cover == true)
            {
                if (cover_type == 3)
                {
                    cover = false;
                    unset_cover();
                }
            }
        }
    }
}
