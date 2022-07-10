using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class power_pick_up : MonoBehaviour
{
    public int type = 0;
    public GameObject sound;

    private void Start()
    {
        sound = GameObject.FindGameObjectWithTag("AudioManager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            sound.GetComponent<audioManager>().play_pick_up();
            collision.gameObject.GetComponent<player_adventure>().unlock_power(type);
            Destroy(this.gameObject);
        }
    }
}
