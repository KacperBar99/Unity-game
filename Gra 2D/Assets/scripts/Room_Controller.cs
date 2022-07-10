using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Controller : MonoBehaviour
{
    

    public bool is_active = false;
    public PolygonCollider2D bound;
    private Vector2 right_door=new Vector2(31f,8.5f);
    private Vector2 left_door=new Vector2(0f,8.5f);
    private Vector2 up_door=new Vector2(15.5f,17f);
    private Vector2 down_door=new Vector2(15.5f,0f);

    bool set = false;
    public GameObject gameController;
    
    private Level_generating templates;
    public int type_id;
    public bool visited=false;

    public GameObject player1;
    public GameObject player2;
    public int grid_position_x;
    public int grid_position_y;
    public bool up_opening;
    public bool down_opening;
    public bool right_opening;
    public bool left_opening;


    public bool is_destination=false;
    public bool is_on_path = false;
    public int exact_id;
    //wskazania na pokoje
    public GameObject left_room=null;
    public GameObject up_room=null;
    public GameObject right_room=null;
    public GameObject down_room=null;


    //public bool spawned=false;
    public GameObject BackGround;
    public List<GameObject> floors;
    public List<GameObject> walls;
    public List<GameObject> celing;


    public GameObject room_setup;
    void Start()
    {
        set = false;// czy ju¿ gotowy
        visited = false;//czy odwiedzony przez gracza
        templates = GameObject.FindGameObjectWithTag("Room_Generator").GetComponent<Level_generating>();//level generator
        grid_position_x = Mathf.RoundToInt(transform.position.x) / 32;//pozycja w siatce
        grid_position_y = Mathf.RoundToInt(transform.position.y) / 18;//pozycja w siatce
        player1 = templates.player1.gameObject;//pierwszy gracz
        if (templates.player2 != null) player2 = templates.player2.gameObject;//gracz drugi opcjonalny
        exact_id = templates.Rooms.Count;//unikatowe id pomieszczenia
        templates.Rooms.Add(this.gameObject);//dodanie pomieszczenia do listy pomieszczeñ
        templates.current_rooms = templates.Rooms.Count;//zwiêkszenie liczby obecnych pomieszczeñ
        gameController = GameObject.FindGameObjectWithTag("GameController");//odniesienie do kontrollera gry

        Invoke("check_neighbours", .5f); //sprawdzenie potencjalnych s¹siadów
    }
    public void set_visited()
    {
        if(visited==false)
        visited = true;
    }
    public void deactivate()
    {
        is_active = false;
        if (room_setup != null)
            room_setup.GetComponent<Room_Setup>().Room_Disable();
       
        if(BackGround!=null)
        BackGround.GetComponent<Animator>().enabled = false;
    }
    public void activate()
    {
        is_active = true;
        if(room_setup!=null)
        room_setup.GetComponent<Room_Setup>().Room_enable();

        if (gameController != null)
        {
            if(BackGround!=null)
            gameController.GetComponent<game_controller>().mover.position = BackGround.transform.position;
        }
        if (BackGround != null)
        {
            BackGround.GetComponent<Animator>().enabled = true;
        }
        else Invoke("activate",0.25f);
    }
    private void check_if_blocked(GameObject neighboor, GameObject door)
    {
        if (neighboor.GetComponent<Room_Controller>() == null) return;


        if (neighboor.GetComponent<Room_Controller>().is_on_path == true || (Random.Range(0, 10) > 5 && is_destination == false) )
        {
            int type = -1;
            switch (neighboor.GetComponent<Room_Controller>().type_id)
            {
                case 0:
                    break;
                case 1:
                    if (neighboor.GetComponent<Room_Controller>().is_on_path == false && neighboor.GetComponent<Room_Controller>().is_destination == false && Random.Range(0, 10) > 5)
                    {
                        type = 0;
                    }
                    break;
                case 2:
                    if (Random.Range(0, 10) > 5)
                    {
                        type = 0;
                    }
                    if (neighboor.GetComponent<Room_Controller>().is_on_path == false && neighboor.GetComponent<Room_Controller>().is_destination == false && Random.Range(0, 10) > 5)
                    {
                        type = 1;
                    }
                    break;
                case 3:
                    if (Random.Range(0, 10) > 5)
                    {
                        type = 1;
                    }
                    else type = 0;
                    if (neighboor.GetComponent<Room_Controller>().is_on_path == false && neighboor.GetComponent<Room_Controller>().is_destination == false && Random.Range(0, 10) > 5)
                    {
                        type = 2;
                    }
                    break;
                case 4:
                    if (Random.Range(0, 10) > 5)
                    {
                        type = 0;
                    }
                    else if (Random.Range(0, 10) > 5)
                    {
                        type = 1;
                    }
                    else type = 2;
                    if (neighboor.GetComponent<Room_Controller>().is_on_path == false && neighboor.GetComponent<Room_Controller>().is_destination == false && Random.Range(0, 10) > 5)
                    {
                        type = 3;
                    }
                    break;

            }
            if (is_destination == true)
            {
                type = type_id - 1;
            }
            if (type >= 0 && type <= 3)
            {
                door.GetComponent<Doors>().set_cover(type);

            }

        }
    }
    
    private void Update()
    {
        if (gameController.GetComponent<game_controller>().Pause == true) return;
        
        //generowanie elemtow pomieszczenia
        if(set==false && templates.done_generating==true)
        {
            set = true;
            //umieszczenia t³a odpowiedniego typu
            Vector3 background_position = new Vector3(transform.position.x + 15.4989f, transform.position.y + 8.5009f, transform.position.z + 1);

            switch (type_id)
            {
                case 0:
                    if (grid_position_x == 0 && grid_position_y == 0) BackGround = Instantiate(templates.backGrounds_standard[0], background_position, Quaternion.identity);
                    else if (is_on_path == true || Random.Range(0,3)==2) BackGround = Instantiate(templates.backGrounds_standard[1], background_position, Quaternion.identity);
                    else BackGround = Instantiate(templates.backGrounds_standard[2], background_position, Quaternion.identity);
                    BackGround.transform.parent = this.transform;
                    break;
                case 1:
                    if (is_destination) BackGround = Instantiate(templates.backGrounds_water[0], background_position, Quaternion.identity);
                    else if (is_on_path == true) BackGround = Instantiate(templates.backGrounds_water[1], background_position, Quaternion.identity);
                    else BackGround = Instantiate(templates.backGrounds_water[2], background_position, Quaternion.identity);
                    BackGround.transform.parent = this.transform;
                    break;
                case 2:
                    if (is_destination) BackGround = Instantiate(templates.backGrounds_earth[0], background_position, Quaternion.identity);
                    else if (is_on_path == true) BackGround = Instantiate(templates.backGrounds_earth[1], background_position, Quaternion.identity);
                    else BackGround = Instantiate(templates.backGrounds_earth[2], background_position, Quaternion.identity);
                    BackGround.transform.parent = this.transform;
                    break;
                case 3:
                    if (is_destination) BackGround = Instantiate(templates.backGrounds_fire[0], background_position, Quaternion.identity);
                    else if (is_on_path == true) BackGround = Instantiate(templates.backGrounds_fire[1], background_position, Quaternion.identity);
                    else BackGround = Instantiate(templates.backGrounds_fire[2], background_position, Quaternion.identity);
                    BackGround.transform.parent = this.transform;
                    break;
                case 4:
                    if (is_destination) BackGround = Instantiate(templates.backGrounds_air[0], background_position, Quaternion.identity);
                    else if (is_on_path == true) BackGround = Instantiate(templates.backGrounds_air[1], background_position, Quaternion.identity);
                    else BackGround = Instantiate(templates.backGrounds_air[2], background_position, Quaternion.identity);
                    BackGround.transform.parent = this.transform;
                    break;
            }
            //sprawdzanie czy jest to pomieszczenie startowe
            if(grid_position_x==0 && grid_position_y==0)
            {
                var tmp_room = Instantiate(templates.first_room, transform.position, Quaternion.identity);
                tmp_room.transform.parent = this.transform;
                room_setup = tmp_room;
            } //losowanie zawartosci pomieszczenia
            else if(is_destination==false)
            {
                GameObject to_spawn;
                
                switch(type_id)
                {
                    case 0:
                       to_spawn = templates.standard_rooms[Random.Range(0, templates.standard_rooms.Count)];
                        break;
                    case 1:                 
                        to_spawn = templates.water_rooms[Random.Range(1, templates.water_rooms.Count)];
                        break;
                    case 2:                     
                        to_spawn = templates.ground_rooms[Random.Range(1, templates.ground_rooms.Count)];
                        break;
                    case 3:
                        to_spawn = templates.fire_rooms[Random.Range(1, templates.fire_rooms.Count)];
                        break;
                    case 4:
                        to_spawn = templates.air_rooms[Random.Range(1, templates.air_rooms.Count)];
                        break;
                    default:
                        to_spawn= templates.standard_rooms[Random.Range(0, templates.standard_rooms.Count)];
                        break;
                }
                var tmp = Instantiate(to_spawn, transform.position, Quaternion.identity);
                tmp.transform.parent = this.transform;
                room_setup = tmp;
                
            }//umieszczanie elementów pomieszczenia docelowego
            else if(is_destination==true)
            {
                GameObject to_spawn;
                switch(type_id)
                { 
                    case 1:
                        to_spawn = templates.water_rooms[0];   
                        break;
                    case 2:
                        to_spawn = templates.ground_rooms[0];
                        break;
                    case 3:
                        to_spawn = templates.fire_rooms[0];
                        break;
                    case 4:
                        to_spawn = templates.air_rooms[0];
                        break;
                    default:
                        to_spawn = templates.water_rooms[0];
                        break;
                }
                var tmp = Instantiate(to_spawn, transform.position, Quaternion.identity);
                tmp.transform.parent = this.transform;
                room_setup = tmp;
            }

            //wstawianie drzwi i ich blokad
            if (left_room != null)
            {
                left_door.x += transform.position.x;
                left_door.y += transform.position.y;
                var tmp = Instantiate(templates.GetComponent<Level_generating>().door, left_door, Quaternion.Euler(0f, 180f, 0f));
                tmp.transform.parent = transform;

                tmp.GetComponent<Doors>().set_teleport(0);

                Vector2 tmp2 = new Vector2(0, 1);
                tmp2.x += transform.position.x;
                tmp2.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_side_open, tmp2, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    walls.Add(tile);
                }
                fill.GetComponent<filler_room>().flip_left();

                //potencjalne wstawianie blokady drzwi

                check_if_blocked(left_room,tmp);
                

            }
            if (right_room != null)
            {
                right_door.x += transform.position.x;
                right_door.y += transform.position.y;
                var tmp = Instantiate(templates.GetComponent<Level_generating>().door, right_door, Quaternion.identity);
                tmp.transform.parent = transform;
                tmp.GetComponent<Doors>().set_teleport(1);

                Vector2 tmp2 = new Vector2(31, 1);
                tmp2.x += transform.position.x;
                tmp2.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_side_open, tmp2, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    walls.Add(tile);
                }


                check_if_blocked(right_room, tmp);
               
            }
            if (up_room != null)
            {
                up_door.x += transform.position.x;
                up_door.y += transform.position.y;
                var tmp = Instantiate(templates.GetComponent<Level_generating>().door, up_door, Quaternion.Euler(0f, 0f, 90f));
                tmp.transform.parent = transform;
                tmp.GetComponent<Doors>().set_teleport(2);

                Vector2 tmp2 = new Vector2(4, 17);
                tmp2.x += transform.position.x;
                tmp2.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_top_open, tmp2, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    celing.Add(tile);
                }
                check_if_blocked(up_room, tmp);
                
            }
            if (down_room != null)
            {
                down_door.x += transform.position.x;
                down_door.y += transform.position.y;
                var tmp = Instantiate(templates.GetComponent<Level_generating>().door, down_door, Quaternion.Euler(0f, 0f, -90f));
                tmp.transform.parent = transform;
                tmp.GetComponent<Doors>().set_teleport(3);

                Vector2 tmp2 = new Vector2(4, 0);
                tmp2.x += transform.position.x;
                tmp2.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_top_open, tmp2, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    floors.Add(tile);
                }
                check_if_blocked(down_room, tmp);
               
            }

            //wstawianie wypelnien dziur
            if (left_room==null && left_opening==true)
            {
                Vector2 tmp = new Vector2(0,1);
                tmp.x += transform.position.x;
                tmp.y += transform.position.y;
                var fill=Instantiate(templates.GetComponent<Level_generating>().filler_side, tmp,Quaternion.identity);
                fill.transform.parent = transform;
                foreach(GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    walls.Add(tile);
                }
                fill.GetComponent<filler_room>().flip_left();
            }
            if(right_room==null && right_opening==true)
            {
                Vector2 tmp = new Vector2(31, 1);
                tmp.x += transform.position.x;
                tmp.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_side, tmp, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    walls.Add(tile);
                }
            }
            if(up_room==null && up_opening==true)
            {
                Vector2 tmp = new Vector2(5,17);
                tmp.x += transform.position.x;
                tmp.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_top, tmp, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    celing.Add(tile);
                }
            }
            if(down_room==null && down_opening==true)
            {
                Vector2 tmp = new Vector2(5,0);
                tmp.x += transform.position.x;
                tmp.y += transform.position.y;
                var fill = Instantiate(templates.GetComponent<Level_generating>().filler_top, tmp, Quaternion.identity);
                fill.transform.parent = transform;
                foreach (GameObject tile in fill.GetComponent<filler_room>().tiles)
                {
                    floors.Add(tile);
                }
            }
            //ustawianie sprite'ow tilesow
            
            switch(type_id)
            {
                case 0:
                    
                    foreach (GameObject tile in floors)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Standard[0];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach(GameObject tile in walls)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Standard[1];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach(GameObject tile in celing)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Standard[2];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    break;
                case 1:
                    
                    foreach (GameObject tile in floors)
                    {

                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Water[0];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in walls)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Water[1];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in celing)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Water[2];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    break;
                case 2:
                    
                    foreach (GameObject tile in floors)
                    {

                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Ground[0];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in walls)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Ground[1];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in celing)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Ground[2];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    break;
                case 3:
                    
                    foreach (GameObject tile in floors)
                    {

                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Fire[0];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in walls)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Fire[1];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in celing)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Fire[2];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    break;
                case 4:
                    
                    
                    foreach (GameObject tile in floors)
                    {

                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Air[0];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in walls)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Air[1];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    foreach (GameObject tile in celing)
                    {
                        tile.GetComponent<Change_Sprite>().new_Sprite = templates.Tiles_Air[2];
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    break;
                default: foreach(GameObject tile in floors)
                    {
                        tile.GetComponent<Change_Sprite>().change_Sprite();
                    }
                    break;
            }
          
            
        }

    }
    void check_neighbours()
    {
        int limit = templates.current_rooms;
        GameObject tmp;
        if (left_opening == true && left_room==null)
        {
            for(int i=0;i<limit;i++)
            {
                tmp = templates.Rooms[i];
                if(tmp.GetComponent<Room_Controller>().right_opening==true && tmp.GetComponent<Room_Controller>().right_room==null)
                {
                    if(tmp.GetComponent<Room_Controller>().grid_position_x==grid_position_x-1 && tmp.GetComponent<Room_Controller>().grid_position_y ==grid_position_y )
                    {
                        left_room = tmp;
                        tmp.GetComponent<Room_Controller>().right_room = gameObject;
                        break;
                    }
                }
            }
        }
        if (right_opening == true && right_room==null)
        {
            for(int i=0;i<limit;i++)
            {
                tmp = templates.Rooms[i];
                if(tmp.GetComponent<Room_Controller>().left_opening==true && tmp.GetComponent<Room_Controller>().left_room==null)
                {
                    if(tmp.GetComponent<Room_Controller>().grid_position_x==grid_position_x+1 && tmp.GetComponent<Room_Controller>().grid_position_y==grid_position_y)
                    {
                        right_room = tmp;
                        tmp.GetComponent<Room_Controller>().left_room = gameObject;
                        break;
                    }
                }
            }
        }
        if (up_opening == true && up_room==null)
        {
            for(int i=0;i<limit;i++)
            {
                tmp = templates.Rooms[i];
                if(tmp.GetComponent<Room_Controller>().down_opening==true && tmp.GetComponent<Room_Controller>().down_room==null)
                {
                    if(tmp.GetComponent<Room_Controller>().grid_position_x==grid_position_x && tmp.GetComponent<Room_Controller>().grid_position_y==grid_position_y+1)
                    {
                        up_room = tmp;
                        tmp.GetComponent<Room_Controller>().down_room = gameObject;
                        break;
                    }
                }
            }
        }
        if (down_opening == true && down_room==null)
        {
            
            for(int i=0;i<limit;i++)
            {
                tmp = templates.Rooms[i];
                if (tmp.GetComponent<Room_Controller>().up_opening==true && tmp.GetComponent<Room_Controller>().up_room==null)
                {
                    if(tmp.GetComponent<Room_Controller>().grid_position_x == grid_position_x && tmp.GetComponent<Room_Controller>().grid_position_y==grid_position_y-1)
                    {
                        down_room = tmp;
                        tmp.GetComponent<Room_Controller>().up_room = gameObject;
                        break;
                    }
                }
            }
        }
    }
    public int get_left()
    {
        if (left_room != null) return left_room.GetComponent<Room_Controller>().exact_id;
        else return -1;
    }
    public int get_right()
    {
        if (right_room != null) return right_room.GetComponent<Room_Controller>().exact_id;
        else return -1;
    }
    public int get_up()
    {
        if (up_room != null) return up_room.GetComponent<Room_Controller>().exact_id;
        else return -1;
    }
    public int get_down()
    {
        if (down_room != null) return down_room.GetComponent<Room_Controller>().exact_id;
        else return -1;
    }
    public void Set_back(int id)
    {
        
        if (type_id != 0) return;
        is_on_path = true;
        
        type_id = id+1;
        
        
    }
}
