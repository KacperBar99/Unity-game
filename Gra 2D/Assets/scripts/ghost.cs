using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : MonoBehaviour
{
    public float lifespan = 10f;
    public int speed=100;
    public int damage = 50;
    public bool evil;
    public GameObject good_ghost;
    public Rigidbody2D body;
    public Transform player;
    bool rest;
    private void Start()
    {
        rest = false;
        if(Random.Range(0,10)==0 && evil==true)
        {
            if(good_ghost!=null)
            Instantiate(good_ghost, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
       
        Destroy(this.gameObject, lifespan);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void FixedUpdate()
    {
        if (evil == true)
        {
            if (player.position.x >= this.transform.position.x)
            {
                body.velocity = new Vector2(speed * Time.fixedDeltaTime, body.velocity.y);
            }
            else if (player.position.x <= this.transform.position.x)
            {
                body.velocity = new Vector2(-speed * Time.fixedDeltaTime, body.velocity.y);
            }
            if (player.position.y > this.transform.position.y)
            {
                body.velocity = new Vector2(body.velocity.x, speed * Time.fixedDeltaTime);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x, -speed * Time.fixedDeltaTime);
            }

            if (body.velocity.x < -.1f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (body.velocity.x > .1f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        else
        {
            body.velocity = new Vector2(0, speed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(evil==true)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<player_adventure>().Take_damage(damage, this.transform.position.x);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if(collision.tag=="Player" && rest == false)
            {
                Invoke("Rest_in_peace", .5f);
                rest = true;
            }
        }
        
    }
    void Rest_in_peace()
    {
        Instantiate(good_ghost, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
