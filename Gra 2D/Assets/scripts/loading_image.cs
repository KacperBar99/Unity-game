using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loading_image : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject panel;
    private void Awake()
    {

        panel.GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
