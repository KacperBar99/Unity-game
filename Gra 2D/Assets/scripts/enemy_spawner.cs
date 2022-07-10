using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_spawner : MonoBehaviour
{
    public GameObject death_effect;
    public Animator animator;
    public Transform spawn_point;
    public GameObject spawn;
    public float Spawn_timer = 10f;
    float Spawn_timer_helper=0f;
    public int hp = 100;
    public int max_hp = 100;
    public Slider Hp_bar;
    public GameObject hp_fill;
    public Gradient health_gradient;
    public GameObject Damage_indicator;
    public GameObject sound;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        max_hp = 100;
        sound = GameObject.FindGameObjectWithTag("AudioManager");
    }

    // Update is called once per frame
    void Update()
    {
        Spawn_timer_helper += Time.deltaTime;
        if(Spawn_timer_helper>=1)
        {
            animator.SetBool("spawn",false);
        }
        if(Mathf.Abs(Spawn_timer_helper-Spawn_timer)<=1)
        {
            animator.SetBool("spawn", true);
        }
        if(Spawn_timer_helper>=Spawn_timer)
        {
            Spawn_timer_helper = 0f;
           var tmp= Instantiate(spawn, spawn_point.position, Quaternion.identity);
            transform.parent.GetComponent<Room_Setup>().room_elements.Add(tmp);
            tmp.transform.parent = this.transform.parent;
        }
        if(hp<=0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(this.gameObject);
        Instantiate(death_effect, this.transform.position, Quaternion.identity);
        
    }
    private void FixedUpdate()
    {
        float tmp1 = hp;
        float tmp2 = max_hp;
        Hp_bar.value = tmp1 / tmp2;
       

        hp_fill.GetComponent<Image>().color = health_gradient.Evaluate(Hp_bar.normalizedValue);

    }
    public void Take_damage(int d)
    {
        sound.GetComponent<audioManager>().play_damage();
        hp -= d;
        var info = Instantiate(Damage_indicator, transform.position, Quaternion.identity);
        info.GetComponentInChildren<TextMesh>().text = d.ToString();
        if (hp <= 0)
        {
            Die();
        }
    }
    
}
