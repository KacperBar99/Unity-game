using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Change_Sprite change_Sprite;
    public GameObject room;
    GameObject gameController;

    public GameObject wall_up;
    public GameObject wall_down;
    public GameObject wall_left;
    public GameObject wall_right;


    int type;

    public bool change = false;
    
    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        Invoke("Update_walls", 1f);
    }
    void Update_walls()
    {
        if(room.GetComponent<Room_Controller>().up_room!=null)
        {
            Destroy(wall_up);
        }
        if(room.GetComponent<Room_Controller>().down_room !=null)
        {
            Destroy(wall_down);
        }
        if(room.GetComponent<Room_Controller>().left_room != null)
        {
            Destroy(wall_left);
        }
        if(room.GetComponent<Room_Controller>().right_room != null)
        {
            Destroy(wall_right);
        }
    }
    private void Update()
    {
        if (gameController.GetComponent<game_controller>().Pause == true) return;

        if(change==false && (room.GetComponent<Room_Controller>().visited==true || room.GetComponent<Room_Controller>().is_destination==true))
        {
            change = true;
            switch (room.GetComponent<Room_Controller>().type_id)
            {
                case 0: gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case 2:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case 3:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 4: gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
                    break;
            }
            if(room.GetComponent<Room_Controller>().is_destination==false)
            {
                if (wall_down != null)
                {

                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_down.GetComponent<SpriteRenderer>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
                }
                if (wall_up != null)
                {
                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_up.GetComponent<SpriteRenderer>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
                }
                if (wall_left != null)
                {
                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_left.GetComponent<SpriteRenderer>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
                }
                if (wall_right != null)
                {
                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_right.GetComponent<SpriteRenderer>().color = new Color(tmp.r / 4, tmp.g / 4, tmp.b / 4);
                }
            }
            else
            {
                
                if (wall_down != null)
                {

                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_down.GetComponent<SpriteRenderer>().color = new Color(tmp.r, tmp.g , tmp.b);
                }
                if (wall_up != null)
                {
                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_up.GetComponent<SpriteRenderer>().color = new Color(tmp.r , tmp.g , tmp.b );
                }
                if (wall_left != null)
                {
                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_left.GetComponent<SpriteRenderer>().color = new Color(tmp.r , tmp.g , tmp.b);
                }
                if (wall_right != null)
                {
                    Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
                    wall_right.GetComponent<SpriteRenderer>().color = new Color(tmp.r , tmp.g , tmp.b);
                }
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
   
        }
        

    }

}
