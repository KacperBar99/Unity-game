using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    public int level = 0;
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 10;
    public float freeze_time = 10f;

    private void Start()
    {
        damage = (int)((level + 1)/10f+1f);
        
        rb.velocity = transform.right * speed;
        Destroy(this.gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.name);

        if (collision.tag == "Enemy")
        {
            if(collision.GetComponent<Enemy>()!=null)
            {
                collision.GetComponent<Enemy>().take_damage(damage);
                collision.GetComponent<Enemy>().freeze(freeze_time);
            }

            

        }
        if(collision.tag=="power rocks")
        {
            if (collision.GetComponent<power_rock>().type == 0)
                collision.GetComponent<power_rock>().end();
        }
        if(collision.tag== "Door")
        {
            if(collision.GetComponent<Doors>().cover==true)
            {
                if(collision.GetComponent<Doors>().cover_type==0)
                {
                    collision.GetComponent<Doors>().cover = false;
                    collision.GetComponent<Doors>().unset_cover();
                }
            }
        }
    }
}
