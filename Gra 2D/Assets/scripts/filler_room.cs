using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class filler_room : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] tiles;

    public void flip_left()
    {
        foreach(GameObject tile in tiles)
        {
            tile.GetComponent<Change_Sprite>().flip_X();
        }
    }
}
