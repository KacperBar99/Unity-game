using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Spawner : MonoBehaviour
{

    public Room_Controller controller;
    public int openingDirection;
    private Level_generating templates;
    private int rand;
    public bool spawned = false;
    float delay=0.1f;

    public float wait_to_die=10f;
    void Start()
    {
        
        controller = transform.parent.gameObject.GetComponent<Room_Controller>();
        templates = GameObject.FindGameObjectWithTag("Room_Generator").GetComponent<Level_generating>();
        delay = templates.delay;
        Invoke("Spawn",delay);

        Invoke("Cleaning", 10f);
       
    }
    void Cleaning()
    {
        Destroy(gameObject,10f);
    }
    void Spawn()
    {
        if (templates == null)
        { 
            
            templates = GameObject.FindGameObjectWithTag("Room_Generator").GetComponent<Level_generating>();
            
        }
        if(controller==null)
        {
           
            controller= transform.parent.gameObject.GetComponent<Room_Controller>();
           
        }
        if (spawned == false && templates.current_rooms <= templates.desired_Rooms)
        {
           // templates.current_rooms++;
            switch (openingDirection)
            {
                case 1://Need to spawn bottom door room
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    var tmp = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                    controller.up_room = tmp;
                    tmp.GetComponent<Room_Controller>().down_room = transform.parent.gameObject;
                    spawned = true;

                    break;
                case 2: //need to spawn top door room 
                    rand = Random.Range(0, templates.topRooms.Length);
                    var tmp1 = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                    controller.down_room = tmp1;
                    tmp1.GetComponent<Room_Controller>().up_room = transform.parent.gameObject;
                    spawned = true;
                    break;
                case 3://nedd to spawn left door room
                    rand = Random.Range(0, templates.leftRooms.Length);
                    var tmp2=Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                    controller.right_room = tmp2;
                    tmp2.GetComponent<Room_Controller>().left_room = transform.parent.gameObject;
                    spawned = true;
                    break;
                case 4: //need to spawn right door room
                    rand = Random.Range(0, templates.rightRooms.Length);
                    var tmp3=Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                    controller.left_room = tmp3;
                    tmp3.GetComponent<Room_Controller>().right_room = transform.parent.gameObject;
                    spawned = true;
                    break;
            }
        } 
        else if (spawned==false)
        {
            templates.set_path = true;
           
        
            switch (openingDirection)
            {
                
                case 1:
                   
                    var temp1 = Instantiate(templates.botomClosed[0], transform.position, templates.botomClosed[0].transform.rotation);
                    controller.up_room = temp1;
                    temp1.GetComponent<Room_Controller>().down_room = transform.parent.gameObject;
                    spawned = true;
                    break;
                case 2:
                    
                     var temp2 = Instantiate(templates.topClosed[0], transform.position, templates.topClosed[0].transform.rotation);
                    controller.down_room = temp2;
                    temp2.GetComponent<Room_Controller>().up_room = transform.parent.gameObject;
                    spawned = true;
                    break;
                case 3:
                    
                     var temp3 =Instantiate(templates.leftClosed[0], transform.position, templates.leftClosed[0].transform.rotation);
                    controller.right_room = temp3;
                    temp3.GetComponent<Room_Controller>().left_room = transform.parent.gameObject;
                    spawned = true;
                    break;
                case 4:
                    
                    var temp4= Instantiate(templates.rightClosed[0], transform.position, templates.rightClosed[0].transform.rotation);
                    controller.left_room = temp4;
                    temp4.GetComponent<Room_Controller>().right_room = transform.parent.gameObject;
                    spawned = true;
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Spawn_Room"))
        {
            
            
            if (collision.GetComponent<Room_Spawner>().spawned == false && spawned==false)
            {
                collision.GetComponent<Room_Spawner>().spawned = true;
                Spawn();
            }
            else spawned = true;
           
        }
        else if(collision.CompareTag("Room_Generator"))
        {
            Destroy(gameObject);
            spawned = true;
           
        }
        
    }
}
