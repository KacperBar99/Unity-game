using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class head_projectile : MonoBehaviour
{
    public GameObject effect;
    public int damage = 10;
    public AIDestinationSetter setter;
    private void Awake()
    {
        setter.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player") 
            {
            
            collision.gameObject.GetComponent<player_adventure>().Take_damage(damage, this.transform.position.x);
            Destroy(this.gameObject);
            Instantiate(effect, this.transform.position, Quaternion.identity);
        }
        if (collision.tag == "Walls" || collision.tag=="platform" || collision.tag=="power rocks") 
        {
            Destroy(this.gameObject);
            Instantiate(effect, this.transform.position, Quaternion.identity);
        }
        
            
    }
}
