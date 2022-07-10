using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_location : MonoBehaviour
{
    
    public float sprite_blink_time=1f;
    private float sprite_blink_time_helper = 0f;
    bool is_visible;

    public GameObject gameController;
    public GameObject player;
    private void Awake()
    {
        is_visible = true;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }
    private void Update()
    {
        if (gameController.GetComponent<game_controller>().Pause == true) return;

        var tmp=new Vector3(player.GetComponent<player_adventure>().grid_position_x, player.GetComponent<player_adventure>().grid_position_y + 1000, -0.5f);
        transform.position = tmp;

        sprite_blink_time_helper += Time.deltaTime;
        Color tmp_c = gameObject.GetComponent<SpriteRenderer>().color;

        if(is_visible)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(tmp_c.r, tmp_c.g, tmp_c.b, tmp_c.a - Time.deltaTime);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(tmp_c.r, tmp_c.g, tmp_c.b, tmp_c.a + Time.deltaTime);
        }

        
        if(sprite_blink_time_helper>=sprite_blink_time)
        {
            sprite_blink_time_helper = 0f;
            is_visible = !is_visible;
        }

    }



 }
