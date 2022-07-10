using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class Level_generating : MonoBehaviour
{
    public GameObject[] power_rocks; 

    public int round;

    public GameObject first_room;
    public List<GameObject> standard_rooms;
    public List<GameObject> water_rooms;
    public List<GameObject> ground_rooms;
    public List<GameObject> fire_rooms;
    public List<GameObject> air_rooms;

    
    public GameObject door;
    
    public GameObject filler_side;
    public GameObject filler_top;
    public GameObject filler_side_open;
    public GameObject filler_top_open;

    public float secod_load = 0f;
    float change_time;
   
    public bool done_generating=false;
    public float delay = 3f;
    public Transform starting_point;
    public GameObject StartRoom;
    public Transform player1;
    public Transform player2;
    public int desired_Rooms;
    public int current_rooms;

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject[] botomClosed;
    public GameObject[] topClosed;
    public GameObject[] leftClosed;
    public GameObject[] rightClosed;


    public List<GameObject> Rooms;

    public GameObject[] backGrounds_standard;
    public GameObject[] backGrounds_water;
    public GameObject[] backGrounds_fire;
    public GameObject[] backGrounds_earth;
    public GameObject[] backGrounds_air;
    


    public Sprite[] Tiles_Standard;
    public Sprite[] Tiles_Water;
    public Sprite[] Tiles_Ground;
    public Sprite[] Tiles_Fire;
    public Sprite[] Tiles_Air;
    public bool set_path = false;
    public bool on_setting_path = false;

    int[,] Graph;

    void Start()
    {
        Time.timeScale = 1f;
        if(round==-1)
        {
            if (Scene_static_sync.sync_round <= 20)
            {
                desired_Rooms = Scene_static_sync.sync_round * 10 + 20;
            }
            else
            {
                desired_Rooms = 200;
            }
        }
        else
        {
            if (round <= 20)
            {
                desired_Rooms = round * 10 + 20;
            }
            else desired_Rooms = 200;
        }
        
        change_time = 0f;
        var tmp=Instantiate(StartRoom, starting_point.position, starting_point.rotation);
        tmp.GetComponent<Room_Controller>().type_id = -1;
        tmp.GetComponent<Room_Controller>().activate();
        tmp.GetComponent<Room_Controller>().visited = true;
        player1.position = new Vector3(starting_point.position.x + 1, starting_point.position.y + 1, starting_point.position.z);
        player1.gameObject.GetComponent<player_adventure>().active_Room = tmp;
          if(player2!=null)
        player2.position = new Vector3(starting_point.position.x + 1, starting_point.position.y + 1, starting_point.position.z);
        if(player2!=null)
        {
            player2.gameObject.GetComponent<player_adventure>().active_Room = tmp;
        }
        player1.GetComponent<player_adventure>().xp = Scene_static_sync.sync_xp;
        if (Scene_static_sync.sync_hp != 0)
            player1.GetComponent<player_adventure>().Hp = Scene_static_sync.sync_hp;
        else player1.GetComponent<player_adventure>().Hp = 100;

    }
    private void Update()
    {
        change_time += Time.deltaTime;

        if (set_path == false && on_setting_path == false && change_time>=2f)
        {
            SceneManager.LoadScene("SinglePlayer", LoadSceneMode.Single);
            

        }
        
        if(set_path==true && on_setting_path==false)
        {
            on_setting_path = true;
            
            Invoke("Paths", 1f);
            
        }
    }

    void Paths()
    {
       

        int[] locate = new int[4];
        find_rooms(locate);
        int src;
        
        calGraph();
        for(int it=0;it<4;it++)
        {
            secod_load = (it + 1f) / 10f;
            Rooms[locate[it]].GetComponent<Room_Controller>().is_destination = true;
            //Rooms[locate[it]].GetComponent<Room_Controller>().is_on_path = true;
            Rooms[locate[it]].GetComponent<Room_Controller>().type_id = it + 1;

            if (it == 0) src = 0;
            else src = locate[it-1];

            int[] dist = new int[current_rooms];
            int[] prev = new int[current_rooms];
            bool[] visited = new bool[current_rooms];

            for(int i=0;i<current_rooms;i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = 0;
                visited[i] = false;
            }
            dist[src] = 0;
            for(int count=0;count<current_rooms-1;count++)
            {
                int u = miniDist(dist, visited);
                visited[u] = true;
                for(int v=0;v<current_rooms;v++)
                {
                    if (!visited[v] && Graph[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + Graph[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + Graph[u, v];
                        prev[v] = u;
                    }
                }
                if(u==locate[it])
                {
                    break;
                }
            }
                 Create_paths(prev, locate,it);
        }
        
        player1.gameObject.GetComponent<player_movement>().ready = true;
        if(player2!=null)
            player2.gameObject.GetComponent<player_movement>().ready = true;
        done_generating = true;
        player1.gameObject.GetComponent<player_adventure>().active_Room.GetComponent<Room_Controller>().set_visited();

        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<game_controller>().Create_Map();
       foreach(GameObject room in Rooms)
        {
            if (room.GetComponent<Room_Controller>().type_id == 0)
            {
                room.GetComponent<Room_Controller>().type_id = Random.Range(0, 5);
            }
            else if (room.GetComponent<Room_Controller>().type_id == -1) room.GetComponent<Room_Controller>().type_id = 0;
        }


    }
    void calGraph()
    {
        
        Graph = new int[current_rooms, current_rooms];
        for (int i=0;i<current_rooms;i++)
        {
            for(int j=0;j<current_rooms;j++)
            {
                Graph[i, j] = 0;
            }
        }
        for(int i=0;i<current_rooms;i++)
        {
            int tmp;
            tmp= Rooms[i].GetComponent<Room_Controller>().get_left();
            if(tmp!=-1)
            {
                Graph[i, tmp] = 1;
            }
            tmp = Rooms[i].GetComponent<Room_Controller>().get_right();
            if (tmp != -1) Graph[i, tmp] = 1;
            tmp= Rooms[i].GetComponent<Room_Controller>().get_up();
            if (tmp != -1) Graph[i, tmp] = 1;
            tmp= Rooms[i].GetComponent<Room_Controller>().get_down();
            if (tmp != -1) Graph[i, tmp] = 1;
        }
       
    }
   

    int miniDist(int []dist,bool []visited)
    {
        int min = int.MaxValue, min_index = -1;
        for(int v=0;v<current_rooms;v++)
        {
            if(visited[v]==false && dist[v]<=min)
            {
                min = dist[v];
                min_index = v;
            }
        }
        return min_index;
    }
    void Create_paths(int []prev,int[] locate,int target)
    {
        int locate_tmp = locate[target];   
        while(locate_tmp!=0)
        {
            Rooms[locate_tmp].GetComponent<Room_Controller>().Set_back(target);
            locate_tmp = prev[locate_tmp];
        }
        
    }
    void find_rooms(int []locate)
    {
        int max_x = int.MinValue;
        int min_x = int.MaxValue;
        int max_y = int.MinValue;
        int min_y = int.MaxValue;
        bool taken=false;
        for(int i=0;i<current_rooms;i++)
        {
            taken = false;
            if (Rooms[i].GetComponent<Room_Controller>().grid_position_y < min_y)
            {
                for(int j=0;j<4;j++)
                {
                    if(Rooms[i].GetComponent<Room_Controller>().exact_id ==locate[j])
                    {
                        taken = true;
                        break;
                    }
                    
                }
                if(taken==false)
                {
                    min_y = Rooms[i].GetComponent<Room_Controller>().grid_position_y;
                    locate[0] = Rooms[i].GetComponent<Room_Controller>().exact_id;
                }
                
                
            }
            taken = false;
            if (Rooms[i].GetComponent<Room_Controller>().grid_position_x < min_x)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Rooms[i].GetComponent<Room_Controller>().exact_id == locate[j])
                    {
                        taken = true;
                        break;
                    }

                }
                if(taken==false)
                {
                    min_x = Rooms[i].GetComponent<Room_Controller>().grid_position_x;
                    locate[1] = Rooms[i].GetComponent<Room_Controller>().exact_id;
                }
                
                
            }
            taken = false;
            if (Rooms[i].GetComponent<Room_Controller>().grid_position_x > max_x)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Rooms[i].GetComponent<Room_Controller>().exact_id == locate[j])
                    {
                        taken = true;
                        break;
                    }

                }
                if(taken==false)
                {
                    max_x = Rooms[i].GetComponent<Room_Controller>().grid_position_x;
                    locate[2] = Rooms[i].GetComponent<Room_Controller>().exact_id;
                }
                
                
            }
            taken = false;
            if (Rooms[i].GetComponent<Room_Controller>().grid_position_y > max_y)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Rooms[i].GetComponent<Room_Controller>().exact_id == locate[j])
                    {
                        taken = true;
                        break;
                    }

                }
                if(taken==false)
                {
                    max_y = Rooms[i].GetComponent<Room_Controller>().grid_position_y;
                    locate[3] = Rooms[i].GetComponent<Room_Controller>().exact_id;
                }
                
                
            }
        }
    }
    
}

