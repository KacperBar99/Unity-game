using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject sound;
    public int type = 0;

    private void Start()
    {
        sound = GameObject.FindGameObjectWithTag("AudioManager");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            sound.GetComponent<audioManager>().play_pick_up();

            
            switch(Random.Range(0,9))
            {
                case 0:
                    collision.gameObject.GetComponent<player_adventure>().health_back(-1);
                    break;
                case 1:
                    collision.gameObject.GetComponent<player_adventure>().health_back(25);
                    break;
                case 2:
                    collision.gameObject.GetComponent<player_adventure>().health_back(50);
                    break;
                case 3:
                    collision.gameObject.GetComponent<player_adventure>().health_back(75);
                    break;
                case 4:
                    collision.gameObject.GetComponent<player_adventure>().health_back(100);
                    break;
                case 5:
                    collision.gameObject.GetComponent<player_adventure>().health_back(125);
                    break;
                case 6:
                    collision.gameObject.GetComponent<player_adventure>().health_back(150);
                    break;
                case 7:
                    collision.gameObject.GetComponent<player_adventure>().health_back(175);
                    break;
                case 8:
                    collision.gameObject.GetComponent<player_adventure>().health_back(200);
                    break;
                case 9:
                    collision.gameObject.GetComponent<player_adventure>().health_back(-1);
                    break;


            }
            Destroy(this.gameObject);
            
        }
    }

}
