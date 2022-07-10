using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class xp : MonoBehaviour
{
    public int value;
    public AIDestinationSetter setter;
    private void Awake()
    {
        setter.target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<player_adventure>().xp_up(value);
            Destroy(gameObject);
        }
    }
}
