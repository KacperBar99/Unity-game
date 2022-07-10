using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ultimate : MonoBehaviour
{
    public GameObject sound;

    private void Start()
    {
        sound = GameObject.FindGameObjectWithTag("AudioManager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            player_adventure tmp = collision.gameObject.GetComponent<player_adventure>();
            tmp.health_back(-1);
            tmp.power_time_helper = 60;
            tmp.xp_up(20);
            Destroy(this.gameObject);
            sound.GetComponent<audioManager>().play_pick_up();
            Scene_static_sync.bonus += 10;
        }
    }
    
}
