using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_info : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D body;
    public float speed_y;
    public float speed_x;
    public float lifetime;
    
    private void Start()
    {
        body=GetComponent<Rigidbody2D>();
        Destroy(gameObject,lifetime);
        body.velocity = new Vector2(Random.Range(-speed_x,speed_x), speed_y);
    }
}
