using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_drop : MonoBehaviour
{
    private PlatformEffector2D effector;


    private void Start()
    {
        effector = gameObject.GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position,.5f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag=="Player")
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    effector.rotationalOffset = 180f;
                }
                else effector.rotationalOffset = 0f;
            }
        }
    }
}
