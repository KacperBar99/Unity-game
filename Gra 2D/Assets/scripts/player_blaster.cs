using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_blaster : MonoBehaviour
{
    public player_adventure adventure;
    public float delay;
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject hit_effect;
    
    void Start()
    {
        
        rb.velocity = transform.right * speed;
        Destroy(this.gameObject, 10f);
       
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float mult = 1f;
        if (adventure.power_selected == 2) 
        {
            mult = 1.5f;
            
        }
        
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            
            int damage = Random.Range(1,5)+Random.Range(1,5)+adventure.character_level/10;
            
            damage = (int)(mult*damage);
            enemy.take_damage(damage);
            Instantiate(hit_effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if(collision.tag=="Player")
        {
            
            
        }
        else if(collision.tag=="Door")
        {
            
            collision.GetComponent<Doors>().activate();
            Instantiate(hit_effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(collision.tag=="Walls")
        {
            Instantiate(hit_effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if(collision.tag=="power rocks")
        {
            Instantiate(hit_effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.tag=="spawner")
        {
            Instantiate(hit_effect, transform.position, transform.rotation);
            Destroy(gameObject);
            int damage = Random.Range(1, 5) + Random.Range(1, 5) + adventure.character_level / 10;
            damage = (int)(mult * damage);
            collision.GetComponent<enemy_spawner>().Take_damage(damage);
        }
        
            
        
        
    }
}
