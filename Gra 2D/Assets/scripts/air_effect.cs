using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air_effect : MonoBehaviour
{
    public float force = 1000;
    public int level = 0;
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 500;

    private void Start()
    {
        damage = (int)((level + 1)/10f+1f);
        rb.velocity = transform.right * speed;
        Destroy(this.gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.name);
        if (collision.tag=="Enemy")
        {
            if (collision.GetComponent<Enemy>() == null) return;
            collision.GetComponent<Enemy>().take_damage(damage);
            if(transform.position.x>collision.transform.position.x)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-force, 0f));
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0f));
            }
            
            
        }
        if (collision.tag == "power rocks")
        {
            if (collision.GetComponent<power_rock>().type == 3)
                collision.GetComponent<power_rock>().end();
        }
        if (collision.tag == "Door")
        {
            if (collision.GetComponent<Doors>().cover == true)
            {
                if (collision.GetComponent<Doors>().cover_type == 3)
                {
                    collision.GetComponent<Doors>().cover = false;
                    collision.GetComponent<Doors>().unset_cover();
                }
            }
        }
    }
}
