using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Setup : MonoBehaviour
{
    public List<GameObject> room_elements;
    private void Start()
    {
        Room_Disable();
    }

    public void Room_Disable()
    {
        foreach(GameObject gameObject in room_elements)
        {
            if(gameObject!=null)
            gameObject.SetActive(false);
        }
    }
    public void Room_enable()
    {
        foreach (GameObject gameObject in room_elements)
        {
            if (gameObject != null)
                gameObject.SetActive(true);
        }
    }
}
