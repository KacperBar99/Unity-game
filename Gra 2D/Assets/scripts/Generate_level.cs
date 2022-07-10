using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Generate_level : MonoBehaviour
{
    public Transform player;

    public Transform starting_point;
    public GameObject central_room;
    public GameObject left_up_room;
    public GameObject right_up_room;
    public GameObject left_down_room;
    public GameObject right_down_room;
    public GameObject central_up_room;
    public GameObject central_down_room;
    public GameObject central_left_room;
    public GameObject central_right_room;
    
    // public GameObject player_prefab;
    // Start is called before the first frame update
    void Start()
    {
        int size = 10;
        int range = 100;
        UnityEngine.Random.InitState(42);
        int bol;
        int[,] Map = new int[size,size];
        var Base = new Vector3(starting_point.position.x,starting_point.position.y,starting_point.position.z);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == 0 && j == 0) Map[i, j] = 1;
                else if (i == 0 && j == size - 1) Map[i, j] = 2;
                else if (i == size - 1 && j == 0) Map[i, j] = 3;
                else if (i == size - 1 && j == size - 1) Map[i, j] = 4;
                else if (j == 0) Map[i, j] = 5;
                else if (j == size - 1) Map[i, j] = 6;
                else if (i == size - 1) Map[i, j] = 7;
                else if (i == 0) Map[i, j] = 8;
                else Map[i, j] = 0;
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (Map[i, j] == 0)
                {
                    bol = UnityEngine.Random.Range(9, range);
                    Map[i, j] = bol;
                }
                starting_point.position = Base; 
                starting_point.position= new Vector3(starting_point.position.x+i*32, starting_point.position.y+j*18, starting_point.position.z);
                
                
                switch(Map[i,j])
                {
                    case 1: Instantiate(left_down_room, starting_point.position, starting_point.rotation);
                        break;
                    case 2: Instantiate(left_up_room, starting_point.position, starting_point.rotation);
                        break;
                    case 3: Instantiate(right_down_room, starting_point.position, starting_point.rotation);
                        break;
                    case 4: Instantiate(right_up_room, starting_point.position, starting_point.rotation);
                        break;
                    case 5: Instantiate(central_down_room, starting_point.position, starting_point.rotation);
                        break;
                    case 6: Instantiate(central_up_room, starting_point.position, starting_point.rotation);
                        break;
                    case 7: Instantiate(central_right_room, starting_point.position, starting_point.rotation);
                        break;
                    case 8: Instantiate(central_left_room, starting_point.position, starting_point.rotation);
                        break;
                    default: Instantiate(central_room, starting_point.position, starting_point.rotation);break; 
                }
               
                
            }
            
        }
        starting_point.position = Base;
        player.position = Base;
        player.position = new Vector3(player.position.x + 2, player.position.y + 2, player.position.z);
        using (var writer = new StreamWriter(@"mapa.txt"))

                for (int i = size-1; i>=0 ; i--)
                {
                    for (int j =0;j<size ; j++)
                    {
                        writer.Write(Map[j,i].ToString());
                        writer.Write(" ");
                    }
                writer.WriteLine("");
                }
                        

            
           
        


    }

   
}
