using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air : MonoBehaviour
{
    public GameObject air_effect;
    public int level=0;
    
    private void Start()
    {

        var tmp1 = Instantiate(air_effect, this.transform.position,Quaternion.identity);
        var tmp2 = Instantiate(air_effect, this.transform.position, Quaternion.Euler(0f,180f,0f));
        tmp1.GetComponent<air_effect>().level = level;
        tmp2.GetComponent<air_effect>().level = level;
        Destroy(this.gameObject, 1f);
        transform.position = new Vector2(transform.position.x, transform.position.y - .5f);
        
    }
    

}
