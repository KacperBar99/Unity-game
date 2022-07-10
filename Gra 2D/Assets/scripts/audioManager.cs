using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class audioManager : MonoBehaviour
{
    public AudioSource source;

    public AudioClip teleport;
   
    public AudioClip pick_up;
    public AudioClip damage;
    

    public void play_teleport()
    {
        source.clip = teleport;
        source.Play();
    }
    
    public void play_pick_up()
    {
        source.clip = pick_up;
        source.Play();
    }
    public void play_damage()
    {
        source.clip = damage;
        source.Play();
    }
    
}
