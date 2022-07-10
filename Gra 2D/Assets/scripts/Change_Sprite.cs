using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Change_Sprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite new_Sprite;

    public void change_Sprite()
    {
        spriteRenderer.color = Color.white;

        if (new_Sprite == null) return;
        spriteRenderer.sprite = new_Sprite;
        
    }
    public void flip_X()
    {
        spriteRenderer.flipX = true;
    }
}
