using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class boss_projectile : MonoBehaviour
{
    public GameObject effect;
    public int damage = 50;
    public float speed = 10f;
    public Rigidbody2D rb;
    public AIDestinationSetter setter;
    private void Start()
    {
        setter.target= GameObject.FindGameObjectWithTag("Player").transform;
        //rb.velocity = -transform.up * speed;
        Destroy(this.gameObject, 2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Instantiate(effect, this.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<player_adventure>().Take_damage(damage, this.transform.position.x);
            Destroy(this.gameObject);
        }
        if (collision.tag == "Walls" || collision.tag == "platform" || collision.tag == "power rocks")
        {
            Instantiate(effect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }


    }
}
