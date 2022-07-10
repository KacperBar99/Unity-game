using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly_shoot : MonoBehaviour
{
    public GameObject effect;

    public bool attention = false;
    public float attention_timer = 1f;
    public float attention_timer_helper = 0f;

    public Transform target;
    public Transform player;
    public float lifespan = 10f;
    public Rigidbody2D body;
    public int speed = 100;
    public int damage = 50;
    public List<Transform> points;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = points[Random.Range(0, points.Count - 1)];
    }

    private void Update()
    {
        if(attention==false)
        {
            attention_timer_helper += Time.deltaTime;
            if(attention_timer_helper>=attention_timer)
            {
                attention = true;
                target = player;
                attention_timer_helper = 0f;
            }
        }

        
    }
    private void FixedUpdate()
    {
        if (target.position.x >= this.transform.position.x)
        {
            body.velocity = new Vector2(speed * Time.fixedDeltaTime, body.velocity.y);
        }
        else if (target.position.x <= this.transform.position.x)
        {
            body.velocity = new Vector2(-speed * Time.fixedDeltaTime, body.velocity.y);
        }
        if (target.position.y > this.transform.position.y)
        {
            body.velocity = new Vector2(body.velocity.x, speed * Time.fixedDeltaTime);
        }
        else
        {
            body.velocity = new Vector2(body.velocity.x, -speed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="platform" || collision.tag=="Walls" || collision.tag=="Door")
        {
            Instantiate(effect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if(collision.tag=="Player")
        {
            Instantiate(effect, this.transform.position, Quaternion.identity);
            collision.GetComponent<player_adventure>().Take_damage(damage,transform.position.x,true);
            Destroy(this.gameObject);
        }
    }
}
