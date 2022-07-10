using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public int direction;
    public Transform teleport_to;
    public GameObject sound;
    private void Start()
    {
        sound = GameObject.FindGameObjectWithTag("AudioManager");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //0 left
        //1 right
        //2 up
        //3 down
        
        
        if (collision.tag == "Player")
        {
            sound.GetComponent<audioManager>().play_teleport();
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.position = teleport_to.position;
            gameObject.GetComponentInParent<Doors>().change_active();
            collision.GetComponent<player_adventure>().active_Room.GetComponent<Room_Controller>().deactivate();
            switch (direction)
            {
                case 0:
                    collision.GetComponent<player_adventure>().active_Room = transform.parent.transform.parent.gameObject.GetComponent<Room_Controller>().left_room;
                    break;
                case 1:
                    collision.GetComponent<player_adventure>().active_Room = transform.parent.transform.parent.gameObject.GetComponent<Room_Controller>().right_room;
                    break;
                case 2:
                    collision.GetComponent<player_adventure>().active_Room = transform.parent.transform.parent.gameObject.GetComponent<Room_Controller>().up_room;
                    break;
                case 3:
                    collision.GetComponent<player_adventure>().active_Room = transform.parent.transform.parent.gameObject.GetComponent<Room_Controller>().down_room;
                    break;
            }
            collision.GetComponent<player_adventure>().active_Room.GetComponent<Room_Controller>().activate();
            collision.GetComponent<player_adventure>().active_Room.GetComponent<Room_Controller>().set_visited();
            collision.GetComponent<player_adventure>().grid_position_x = collision.GetComponent<player_adventure>().active_Room.GetComponent<Room_Controller>().grid_position_x;
            collision.GetComponent<player_adventure>().grid_position_y = collision.GetComponent<player_adventure>().active_Room.GetComponent<Room_Controller>().grid_position_y;
            
        }
        




    }
}
