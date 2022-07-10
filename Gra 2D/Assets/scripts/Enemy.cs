using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
public class Enemy : MonoBehaviour
{

    public float attention_timer=5f;
    public float attention_timer_helper = 0f;
    public bool attention = true;

    public GameObject health;
    int boss_multiplier=4;
    public List<Transform> points;
    public List<GameObject> room_elements;
    public Rigidbody2D body;

    public GameObject enemy_spawn;
    public GameObject special_death;
    public Animator animator;
    public GameObject projectile;

    public float charge_timer = 10f;
    float charge_timer_helper = 0f;
    bool can_charge = false;

    public float shoot_timer = 15f;
    float shoot_timer_helper = 0f;
    bool can_shoot = false;



    public bool can_attack = true;
    public float attack_timer = 5f;
    public float attack_timer_helper = 0f;

    public int type = 0;

    float move_time = 5f;
    float move_time_helper;
    bool moving = false;
    

    float freeze_t = 0;
    public bool freeze_d = false;
    float freeze_t_helper = 0;

    float extra_t = 0;
    public bool extra_d = false;
    int extra_damage;

    float extra_t_helper = 0;

    public GameObject ice;
    public GameObject fire;
    public GameObject[] xps;

    public AIDestinationSetter setter;
    public GameObject game_Controller;
    public Gradient health_gradient;
    public GameObject hp_fill;
    public Slider Hp_bar;

    
    public int hp = 100;
    public int Max_Hp = 100;
    public GameObject Damage_indicator;

    public GameObject death_effect;

    bool ready = false;
    public GameObject sound;
    public AudioSource zombie_sound;
 
    private void OnEnable()
    {
        if(type==2 && ready==true)
        {
            if (game_Controller != null)
                game_Controller.GetComponent<game_controller>().boss_health(true);
        }
        
    }
    private void OnDisable()
    {
        if (type == 2 && ready==true)
        {
            if(game_Controller!=null)
            game_Controller.GetComponent<game_controller>().boss_health(false);
        }
            
    }

    public AIPath aiPath;
    GameObject player;
    private Vector3 m_Velocity = Vector3.zero;
    void Start()
    {
        sound = GameObject.FindGameObjectWithTag("AudioManager");
        game_Controller = GameObject.FindGameObjectWithTag("GameController");
        ready = true;
        if(type==2)
        game_Controller.GetComponent<game_controller>().boss_health(true);
        player = GameObject.FindGameObjectWithTag("Player");
        if (type==0)
        {
            hp = 100;
            Max_Hp = 100;
            setter.target = player.transform;
        }
        if(type==1)
        {
           // aiPath.canMove = true;
            
            hp = 10;
            Max_Hp = 10;
            //setter.target = player.transform;
        }
        if(type==2)
        {
            hp = 250;
            Max_Hp = 250;
            setter.target = player.transform;
            foreach (GameObject gameObject in room_elements)
            {
                if (gameObject != null)
                    gameObject.SetActive(false);
            }
        }
        if(type==3)
        {
            hp = 50;
            Max_Hp = 50;
            setter.target = player.transform;
        }
        
        
    }
    public void take_damage(int damage)
    {
        
        sound.GetComponent<audioManager>().play_damage();
        if (type == 2)
        {
            
            

            hp_fill.GetComponent<Image>().color = health_gradient.Evaluate(Hp_bar.normalizedValue);
            // ratio= (float)damage / (float)Max_Hp;
            // float tmp1 = Max_Hp;
            // float tmp2 = hp;
            //game_Controller.GetComponent<game_controller>().boss_slider_2.value = tmp2/tmp1;
            game_Controller.GetComponent<game_controller>().boss_damage_timer_helper = 0f;

            


            if (hp>=50*boss_multiplier && hp-damage<50*boss_multiplier)
            {
                boss_multiplier--;
                Instantiate(health, transform.position, Quaternion.identity);
            }
        }
        hp -= damage;
        var info=Instantiate(Damage_indicator, transform.position, Quaternion.identity);
        info.GetComponentInChildren<TextMesh>().text = damage.ToString();
        if (hp <= 0)
        {
            Die();
        }
        
    }
    void Die()
    {
        Destroy(gameObject);
        
        switch (type)
        {
            case 0:
                var tmp0 = Instantiate(death_effect, transform.position, Quaternion.identity);
                Destroy(tmp0, 1f);
                Instantiate(xps[Random.Range(0, xps.Length)], transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(xps[0], transform.position, Quaternion.identity);
                if (Random.Range(0, 20)!=0)
                {
                    var tmp1 = Instantiate(death_effect, transform.position, Quaternion.identity);
                    Destroy(tmp1, 1f);
                }
                else
                {
                    var tmp=Instantiate(special_death, transform.position, Quaternion.identity);
                    Destroy(tmp, 1f);
                    tmp = Instantiate(enemy_spawn, transform.position, Quaternion.identity);
                   
                } 
                break;
            case 2:
                foreach (GameObject gameObject in room_elements)
                {
                    if (gameObject != null)
                        gameObject.SetActive(true);
                }
                int lim = Random.Range(1, 10);
                for(int i=0;i<lim;i++)
                {
                    Instantiate(xps[Random.Range(0, xps.Length)], transform.position, Quaternion.identity);
                }
                var tmp2 = Instantiate(death_effect, transform.position, Quaternion.identity);

                Destroy(tmp2, 1f);
                Instantiate(enemy_spawn, transform.position, Quaternion.identity);
                break;
            case 3:
                var tmp3 = Instantiate(death_effect, transform.position, Quaternion.identity);
                Destroy(tmp3, 1f);
                Instantiate(xps[Random.Range(0, xps.Length)], transform.position, Quaternion.identity);
                break;
                
        }
        

    }
    void Update()
    {
        
       switch(type)
        {
            case 0:
                {
                    if (aiPath.desiredVelocity.x >= 0.1f)
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else if (aiPath.desiredVelocity.x <= -0.1f)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    if (can_shoot == false && freeze_d == false)
                    {
                        shoot_timer_helper += Time.deltaTime;
                        if (shoot_timer_helper >= 1)
                        {
                            animator.SetBool("shoot", false);
                        }
                        if (shoot_timer_helper >= shoot_timer - 1)
                        {
                            animator.SetBool("shoot", true);
                        }
                        if (shoot_timer_helper >= shoot_timer)
                        {
                            can_shoot = true;
                            shoot_timer_helper = 0f;

                            Shoot();
                        }
                    }


                    if (can_charge == false)
                    {
                        charge_timer_helper += Time.deltaTime;
                        if (charge_timer_helper >= charge_timer)
                        {
                            aiPath.maxSpeed = 10f;
                            can_charge = true;
                            charge_timer_helper = 0f;
                            animator.SetBool("charge", true);
                        }
                    }
                    else
                    {
                        charge_timer_helper += Time.deltaTime;
                        if (charge_timer_helper >= 1) animator.SetBool("charge", false);
                        if (charge_timer_helper >= charge_timer / 2)
                        {
                            can_charge = false;
                            charge_timer_helper = 0f;
                            aiPath.maxSpeed = 3f;
                        }

                    }
                    if (can_attack == false)
                    {
                        attack_timer_helper += Time.deltaTime;
                        if (attack_timer_helper >= 1) animator.SetBool("bite", false);
                        if (attack_timer_helper >= attack_timer)
                        {
                            can_attack = true;
                            attack_timer_helper = 0f;
                        }
                    }



                    if (attention == false)
                    {
                        attention_timer_helper += Time.deltaTime;
                        if (attention_timer_helper >= attention_timer / 5)
                        {
                            attention_timer_helper = 0f;
                            attention = true;
                            setter.target = player.transform;
                        }
                    }
                    else
                    {
                        attention_timer_helper += Time.deltaTime;
                        if (attention_timer_helper >= attention_timer * 3)
                        {
                            attention = false;
                            attention_timer_helper = 0f;
                            int tmp_min_in = 0;
                            float tmp_d = 0;
                            int it = 0;
                            foreach (Transform ob in points)
                            {
                                if (Vector2.Distance(ob.position, player.transform.position) >= tmp_d)
                                {
                                    tmp_d = Vector2.Distance(ob.position, player.transform.position);
                                    tmp_min_in = it;
                                }
                                it++;
                            }
                            if (points[tmp_min_in] != null)
                                setter.target = points[tmp_min_in];
                            else setter.target = player.transform;
                        }
                    }
                }
                
                


                break;


            case 1:
                {
                    






                    if (can_attack == false)
                    {
                        attack_timer_helper += Time.deltaTime;
                        if (attack_timer_helper >= 1) animator.SetBool("attack", false);
                        if (attack_timer_helper >= attack_timer)
                        {
                            can_attack = true;
                            attack_timer_helper = 0f;
                        }
                    }


                    break;
                }
                
            case 2:
                {
                    if (can_shoot == false && freeze_d == false)
                    {
                        shoot_timer_helper += Time.deltaTime;
                        if (shoot_timer_helper >= 1)
                        {
                            animator.SetBool("shoot", false);
                        }
                        if (shoot_timer_helper >= shoot_timer - 1)
                        {
                            animator.SetBool("shoot", true);
                        }
                        if (shoot_timer_helper >= shoot_timer)
                        {
                            can_shoot = true;
                            shoot_timer_helper = 0f;

                            Shoot();
                        }
                    }
                    if (can_attack == false)
                    {
                        attack_timer_helper += Time.deltaTime;
                        if (attack_timer_helper >= 1) animator.SetBool("roll", false);
                        if (attack_timer_helper >= attack_timer)
                        {
                            can_attack = true;
                            attack_timer_helper = 0f;
                        }
                    }
                    if (attention == false)
                    {
                        attention_timer_helper += Time.deltaTime;
                        if (attention_timer_helper >= attention_timer)
                        {
                            attention_timer_helper = 0f;
                            attention = true;
                            setter.target = player.transform;
                        }
                    }
                    else
                    {
                        attention_timer_helper += Time.deltaTime;
                        if (Vector2.Distance(transform.position, player.transform.position) <= 2 || attention_timer_helper >= attention_timer * 2)
                        {
                            attention = false;
                            attention_timer_helper = 0f;
                            int tmp_min_in = 0;
                            float tmp_d = 0;
                            int it = 0;
                            foreach (Transform ob in points)
                            {
                                if (Vector2.Distance(ob.position, player.transform.position) >= tmp_d)
                                {
                                    tmp_d = Vector2.Distance(ob.position, player.transform.position);
                                    tmp_min_in = it;
                                }
                                it++;
                            }
                            if (points[tmp_min_in] != null)
                                setter.target = points[tmp_min_in];
                            else setter.target = player.transform;
                        }
                    }
                }

                break;
            case 3:
                {
                    if (can_shoot == false && freeze_d == false)
                    {
                        shoot_timer_helper += Time.deltaTime;
                        if (shoot_timer_helper >= 1)
                        {
                            animator.SetBool("shoot", false);
                        }
                        if (shoot_timer_helper >= shoot_timer - 1)
                        {
                            animator.SetBool("shoot", true);
                        }
                        if (shoot_timer_helper >= shoot_timer)
                        {
                            can_shoot = true;
                            shoot_timer_helper = 0f;

                            Shoot();
                        }
                    }
                    if (can_attack == false)
                    {
                        attack_timer_helper += Time.deltaTime;
                        if (attack_timer_helper >= 1)
                        { 
                            animator.SetBool("teleport", false); 
                        }
                        if (attack_timer_helper >= attack_timer)
                        {
                            can_attack = true;
                            attack_timer_helper = 0f;
                        }
                    }
                    if (attention == false)
                    {
                        attention_timer_helper += Time.deltaTime;
                        if (attention_timer_helper >= attention_timer)
                        {
                            attention_timer_helper = 0f;
                            attention = true;
                            setter.target = player.transform;
                        }
                    }
                    else
                    {
                        attention_timer_helper += Time.deltaTime;
                        if (Vector2.Distance(transform.position, player.transform.position) <= 1 || attention_timer_helper >= attention_timer)
                        {
                            attention_timer_helper = 0f;
                            attention= false;
                            if(points.Count>1)
                            {
                                setter.target = points[Random.Range(0, points.Count - 1)];
                            }
                            else setter.target = player.transform;
                        }
                    }
                    break;
                }
               


        }
        
        


        if (game_Controller.GetComponent<game_controller>().Pause == true) return;
        if (hp <= 0) Die();
    }
    void Shoot()
    {
        if (can_shoot == false) return;

        switch(type)
        {
            case 0:
                can_shoot = false;
                Instantiate(projectile, transform.position, Quaternion.identity);
                break;
            case 1:
                can_shoot = false;
                break;
            case 2:
                can_shoot = false;
                Instantiate(projectile, transform.position, Quaternion.identity);
                break;
            case 3:
                can_shoot = false;
                int limit = Random.Range(2, 5);
                for(int i=0;i<limit;i++)
                {
                    Invoke("butter_shoot",0f+(float)i*.2f);
                }
                
                break;
        }
        
    }
    void butter_shoot()
    {
        var tmp=Instantiate(projectile, transform.position, Quaternion.identity);
        foreach(Transform p in points)
        {
            tmp.GetComponent<fly_shoot>().points.Add(p);
        }
        
    }
    void Attack(GameObject ob)
    {
        if (can_attack == false) return;
        switch (type)
        {
            case 0:
                can_attack = false;
                animator.SetBool("bite", true);
                ob.GetComponent<player_adventure>().Take_damage(10, this.transform.position.x);
                break;
            case 1:
                zombie_sound.Play();
                can_attack = false;
                animator.SetBool("attack", true);
                ob.GetComponent<player_adventure>().Take_damage(5, this.transform.position.x);
                break;
            case 2:
                can_attack = false;
                animator.SetBool("roll", true);
                ob.GetComponent<player_adventure>().Take_damage(25, this.transform.position.x);
                break;
            case 3:
                can_attack = false;
                animator.SetBool("teleport", true);
                ob.GetComponent<player_adventure>().Take_damage(50, this.transform.position.x,true);
                break;

        }

        
    }
    private void FixedUpdate()
    {
        float tmp1 = hp;
        float tmp2 = Max_Hp;
        Hp_bar.value = tmp1 / tmp2;
        if (type == 2)
        {
            game_Controller.GetComponent<game_controller>().boss_slider.value = tmp1 / tmp2;
        }
            
        hp_fill.GetComponent<Image>().color = health_gradient.Evaluate(Hp_bar.normalizedValue);

        switch (type)
        {
            case 0:
                {
                    if (extra_d == true)
                    {
                        extra_t -= Time.fixedDeltaTime;
                        if (Mathf.Abs(extra_t - extra_t_helper) >= 1)
                        {
                            extra_t_helper = extra_t;
                            take_damage(extra_damage);
                        }
                        if (extra_t <= 0)
                        {
                            extra_d = false;
                            fire.SetActive(false);
                        }
                    }
                    if (freeze_d == true)
                    {
                        freeze_t_helper += Time.fixedDeltaTime;
                        if (freeze_t_helper >= freeze_t)
                        {
                            animator.speed = 1;
                            aiPath.canMove = true;
                            freeze_d = false;
                            ice.SetActive(false);
                            freeze_t_helper = 0f;
                        }
                    }
                    if (moving == true)
                    {
                        move_time_helper += Time.fixedDeltaTime;
                        if (move_time_helper >= move_time)
                        {
                            moving = false;
                            aiPath.canMove = true;
                            move_time_helper = 0f;
                        }
                    }
                }
                
                break;
            case 1:
                {
                    animator.SetFloat("speed", Mathf.Abs(body.velocity.x));


                    //Debug.Log(transform.position.x + " " + player.transform.position.x +" "+body.velocity);
                    if (freeze_d == false && moving == false)
                    {
                        if (player.transform.position.x >= this.transform.position.x)
                        {
                            body.velocity = new Vector2(100 * 0.0083f, body.velocity.y + .1f * 0.0083f);
                        }
                        else if (player.transform.position.x <= this.transform.position.x)
                        {
                            body.velocity = new Vector2(-100 * 0.0083f, body.velocity.y + .1f * 0.0083f);
                        }
                    }
                    else
                    {
                        body.velocity = new Vector2(0, body.velocity.y);
                    }




                    if (body.velocity.x < -.1f)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else if (body.velocity.x > .1f)
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    if (extra_d == true)
                    {
                        extra_t -= Time.fixedDeltaTime;
                        if (Mathf.Abs(extra_t - extra_t_helper) >= 1)
                        {
                            extra_t_helper = extra_t;
                            take_damage(extra_damage);
                        }
                        if (extra_t <= 0)
                        {
                            extra_d = false;
                            fire.SetActive(false);
                        }
                    }
                    if (freeze_d == true)
                    {
                        freeze_t_helper += Time.fixedDeltaTime;
                        if (freeze_t_helper >= freeze_t)
                        {
                            animator.speed = 1;
                            freeze_d = false;
                            ice.SetActive(false);
                            freeze_t_helper = 0f;
                        }
                    }
                    if (moving == true)
                    {
                        move_time_helper += Time.fixedDeltaTime;
                        if (move_time_helper >= move_time)
                        {
                            moving = false;

                            move_time_helper = 0f;
                        }
                    }
                }

                
                break;
            case 2:
                {
                    if (extra_d == true)
                    {
                        extra_t -= Time.fixedDeltaTime;
                        if (Mathf.Abs(extra_t - extra_t_helper) >= 1)
                        {
                            extra_t_helper = extra_t;
                            take_damage(extra_damage);
                        }
                        if (extra_t <= 0)
                        {
                            extra_d = false;
                            fire.SetActive(false);
                        }
                    }
                    if (freeze_d == true)
                    {
                        freeze_t_helper += Time.fixedDeltaTime;
                        if (freeze_t_helper >= freeze_t)
                        {
                            animator.speed = 1;
                            aiPath.canMove = true;
                            freeze_d = false;
                            ice.SetActive(false);
                            freeze_t_helper = 0f;
                        }
                    }
                    if (moving == true)
                    {
                        move_time_helper += Time.fixedDeltaTime;
                        if (move_time_helper >= move_time)
                        {
                            moving = false;
                            aiPath.canMove = true;
                            move_time_helper = 0f;
                        }
                    }
                }

                
                break;
            case 3:
                {
                    if (extra_d == true)
                    {
                        extra_t -= Time.fixedDeltaTime;
                        if (Mathf.Abs(extra_t - extra_t_helper) >= 1)
                        {
                            extra_t_helper = extra_t;
                            take_damage(extra_damage);
                        }
                        if (extra_t <= 0)
                        {
                            extra_d = false;
                            fire.SetActive(false);
                        }
                    }
                    if (freeze_d == true)
                    {
                        freeze_t_helper += Time.fixedDeltaTime;
                        if (freeze_t_helper >= freeze_t)
                        {
                            animator.speed = 1;
                            aiPath.canMove = true;
                            freeze_d = false;
                            ice.SetActive(false);
                            freeze_t_helper = 0f;
                        }
                    }
                    if (moving == true)
                    {
                        move_time_helper += Time.fixedDeltaTime;
                        if (move_time_helper >= move_time)
                        {
                            moving = false;
                            aiPath.canMove = true;
                            move_time_helper = 0f;
                        }
                    }
                }
                
                break;
            default:
                break;
        }
        
    }
    public void set_extra_time(float time,int d)
    {
        extra_t = time;
        extra_d = true;
        extra_damage = d;
        extra_t_helper = time;
        fire.SetActive(true);
    }
    public void freeze(float time)
    {
        animator.speed = 0;
        // Debug.Log("Hey");
        if (aiPath!=null)
        aiPath.canMove = false;
        ice.SetActive(true);
        freeze_t = time;
        freeze_d = true;
        freeze_t_helper = 0 ;
    }
    public void move_enemy()
    {
        if(aiPath!=null)
        aiPath.canMove = false;
        moving = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(type)
        {
            case 0:
                if(collision.gameObject.tag=="Player")
                {
                    Attack(collision.gameObject);
                }
                break;
            case 1:
                if(collision.gameObject.tag=="Player")
                {
                    Attack(collision.gameObject);
                }
                break;
            case 2:
                if(collision.gameObject.tag=="Player")
                {
                    Attack(collision.gameObject);
                }
                break;
            case 3:
                if(collision.gameObject.tag=="Player")
                {
                    Attack(collision.gameObject);
                }
                break;
        }
    }
}
