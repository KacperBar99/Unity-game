using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class power_rock : MonoBehaviour
{
    public int type;
    public GameObject[] xps;
    public BoxCollider2D box;
    
    public void end()
    {
        Destroy(this.gameObject);
        Instantiate(xps[Random.Range(0, xps.Length)], transform.position, Quaternion.identity);
    }

    private void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);

        switch(type)
        {
            case 0:
                if(collision.gameObject.GetComponent<water>()!=null)
                {
                    Destroy(gameObject);
                }
                break;
            case 1:
                if (collision.gameObject.GetComponent<ground>() != null)
                {
                    Destroy(gameObject);
                }
                break;
            case 2:
                if (collision.gameObject.GetComponent<fire>() != null)
                {
                    Destroy(gameObject);
                }
                break;
            case 3:
                if (collision.gameObject.GetComponent<air>() != null)
                {
                    Destroy(gameObject);
                }
                if (collision.gameObject.GetComponent<player_adventure>() != null && collision.gameObject.GetComponent<player_adventure>().power_selected == 3)
                {
                    Destroy(gameObject);
                }
                break;
            case 4:
                if (collision.gameObject.GetComponent<player_adventure>() != null && collision.gameObject.GetComponent<player_adventure>().power_selected==3)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
}
