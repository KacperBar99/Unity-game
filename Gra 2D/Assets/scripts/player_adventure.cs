using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_adventure : MonoBehaviour
{
    public GameObject xp_prefab;

    public GameObject air_power_ob;
    public GameObject fire_power_ob;
    public GameObject water_power_ob;
    public GameObject ground_power_ob;

    public bool water_power = false;
    public bool ground_power = false;
    public bool fire_power = false;
    public bool air_power = false;

    public float x_stagger;
    public float y_stagger;
    public float stagger_time = 1f;
     float stagger_time_helper = 0f;
     bool stagger=false;

    public GameObject damage_indicator;
    public GameObject health_indicator;

    public int power_selected=-1;
    public GameObject game_Controller;
  
    public Transform[] gun_point_arangement;
    public int shooting_point;
    public GameObject blaster_shoot;
    public player_movement player_Movement;

    public int xp_needed_last = 0;
    public int xp_needed = 10;
    public int character_level=0;
    public int xp=0;
    public int Hp_Max = 100;
    public int Hp = 100;
    public int grid_position_x;
    public int grid_position_y;
    public GameObject active_Room;

    public float fire_time = .25f;
    float fire_time_helper = 0f;
    bool fire = true;
    
    
    public float power_time = 60f;
    public float power_time_helper = 0f;
    bool power = false;
    public GameObject sound;
    
     void Start()
    {
        sound = GameObject.FindGameObjectWithTag("AudioManager");
        fire_time = 1f;
        game_Controller = GameObject.FindGameObjectWithTag("GameController");
        shooting_point = 0;
        Hp_Max = 100;
        Hp = 100;
        grid_position_x = 0;
        grid_position_y = 0;
    }
     void Update()
    {
        if (game_Controller.GetComponent<game_controller>().Pause == true) return;
        if (player_Movement.ready == false) return;
        if(stagger==true)
        {
            stagger_time_helper += Time.deltaTime;
            if(stagger_time_helper>=stagger_time)
            {
                stagger_time_helper = 0f;
                stagger = false;
            }
        }
        if(fire==false)
        {
            if(player_Movement.controller.GetComponent<player_controller>().is_Crouching==true)
            {
                fire_time_helper += Time.deltaTime * 2* (1 + (float)character_level / 99f);
            }
            else 
            fire_time_helper += Time.deltaTime*(1+(float)character_level/99f);
            if(fire_time_helper>=fire_time)
            {
                fire_time_helper = 0f;
                fire = true;
            }
        }
        if(power==false)
        {
            if(power_time_helper>=power_time)
            {
                power_time_helper = 60f;
                power = true;
            }
            else
            {
                float multiplier = 1;
                if (player_Movement.horizontalMove == 0)
                {
                    multiplier *= 1.5f;
                    if (player_Movement.crouch == true) multiplier *= 1.5f;
                }
                power_time_helper += Time.deltaTime * (1 + ((float)character_level + 1f) / 50f)* multiplier;
                
            }
        }
        

        if (player_Movement.ready == false) return;
        if(player_Movement.controller.is_Crouching==true)
        {
            shooting_point = 1;
        }
        else
        {
            shooting_point = 0;
        }
        if(player_Movement.look_up==true)
        {
            if (player_Movement.look_straight == true) shooting_point = 3;
            else shooting_point = 2;
        }
        else if(player_Movement.look_down==true)
        {
            if (player_Movement.look_straight == true) shooting_point = 5;
            else shooting_point = 4;
        }
        if (player_Movement.controller.is_Crouching == true)
        {
            shooting_point = 1;
        }
        if (player_Movement.controller.m_Grounded==true)
        {
            
            if (Input.GetButtonDown("Shoot") && game_Controller.GetComponent<game_controller>().is_map==false)
            {
                if(player_Movement.controller.is_Crouching==false || player_Movement.horizontalMove==0)     Shoot_Blaster();
            }
            else if(Input.GetAxis("Shoot")>0 && game_Controller.GetComponent<game_controller>().is_map==false)
            {
                if (player_Movement.controller.is_Crouching == false || player_Movement.horizontalMove == 0) Shoot_Blaster();
            }
        }
        if(game_Controller.GetComponent<game_controller>().keyboard==true)
        {
            if (Input.GetButtonDown("Water power") && water_power == true)
            {
                power_selected = 0;
            }
            else if (Input.GetButtonDown("Ground power") && ground_power == true)
            {
                power_selected = 1;
            }
            else if (Input.GetButtonDown("Fire power") && fire_power == true)
            {
                power_selected = 2;
            }
            else if (Input.GetButtonDown("Air power") && air_power == true)
            {
                power_selected = 3;
            }
        }
        else
        {
            

            if(Input.GetAxis("Power ver")<-0.1 && water_power==true)
            {
                power_selected = 0;
            }
            else if(Input.GetAxis("Power ver")>0.1 && air_power==true)
            {
                power_selected = 3;
            }
            else if(Input.GetAxis("Power hor")>0.1 && fire_power==true)
            {
                power_selected = 2;
            }
            else if(Input.GetAxis("Power hor")<-0.1 && ground_power==true)
            {
                power_selected = 1;
            }
        }
        
        if(Input.GetButtonDown("Use power") && game_Controller.GetComponent<game_controller>().is_map==false)
        {
            Use_power();
        }

        
    }
    private void FixedUpdate()
    {
        /*
        if(Input.GetButton("hp up"))
        {
            Hp = Hp_Max;
        }
        if(Input.GetButton("level up"))
        {
            xp += 10;
        }
        if(Input.GetButton("Power up"))
        {
            power_time_helper = 59.9f;
        }*/
        

       if(xp>=xp_needed && character_level<99)
        {
            xp_needed_last = xp_needed;
            character_level++;
            xp_needed += 10+character_level;
            Hp_Max += 1;
            
        }
        
        
    }
    void Die()
    {
        player_Movement.ready = false;
        game_Controller.GetComponent<game_controller>().Get_Name();
        player_Movement.set_death();
    }
    public void Take_damage(int damage, float x, bool neg_stag = false)
    {
        float mult = 1f;
        if(power_selected==1)
        {
            mult = .75f;
        }

        if (stagger == true && neg_stag==false)
        {
            return;
        }
        else stagger = true;

        sound.GetComponent<audioManager>().play_damage();
        Rigidbody2D tmp = gameObject.GetComponent<Rigidbody2D>();
        if(power_selected!=1)
        {
            if (x > this.transform.position.x)
            {
                tmp.AddForce(new Vector2(-x_stagger, y_stagger));
            }
            else
            {
                tmp.AddForce(new Vector2(x_stagger, y_stagger));
            }
        }
        
        
        Hp -= (int)(damage*mult);
        var info = Instantiate(damage_indicator, transform.position, Quaternion.identity);
        info.GetComponentInChildren<TextMesh>().text = ((int)(damage * mult)).ToString();
        if (Hp <= 0)
        {
            Die();
        }
    }
    void Use_power()
    {
        if(power==false)
        {
            return;
        }
        else
        {
            switch (power_selected)
            {
                case 0://water
                    var tmp0 = Instantiate(water_power_ob, gun_point_arangement[shooting_point].transform.position, gun_point_arangement[shooting_point].transform.rotation);
                    tmp0.GetComponent<water>().level = character_level;
                    power = false;
                    power_time_helper = 0f;
                    break;
                case 1://ground 
                    var tmp1 = Instantiate(ground_power_ob, gun_point_arangement[shooting_point].transform.position, gun_point_arangement[shooting_point].transform.rotation);
                    tmp1.GetComponent<ground>().level = character_level;
                    power = false;
                    power_time_helper = 0f;
                    break;
                case 2://fire
                    var tmp2 = Instantiate(fire_power_ob, gun_point_arangement[shooting_point].transform.position, gun_point_arangement[shooting_point].transform.rotation);
                    tmp2.GetComponent<fire>().level = character_level;
                    power = false;
                    power_time_helper = 0f;
                    break;
                case 3://air
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
                    

                    var tmp3=Instantiate(air_power_ob,transform.position,Quaternion.identity);
                    tmp3.transform.parent = this.transform;
                    tmp3.GetComponent<air>().level = character_level;
                    power = false;
                    power_time_helper = 0f;
                    break;
                
            }
            
        }

        
    }
    void Shoot_Blaster()
    {
        if(fire==false)
        {
            return;
        }
        else
        {
            fire = false;
            var tmp = Instantiate(blaster_shoot, gun_point_arangement[shooting_point].transform.position, gun_point_arangement[shooting_point].transform.rotation);
            tmp.GetComponent<player_blaster>().adventure = this;
        }

        
    }
    public void xp_up(int v)
    {
       var tmp= Instantiate(xp_prefab, transform.position, Quaternion.identity);
        Destroy(tmp, 1f);
        xp += v;
        power_time_helper += v;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            Take_damage(collision.gameObject.GetComponent<Enemy>().type,collision.transform.position.x);
        }
    }
    public void health_back(int h)
    {
        float mult = 1f;
        if (power_selected == 0) mult = 1.5f;
        if (h != -1)
        {
           
            var info = Instantiate(health_indicator, transform.position, Quaternion.identity);
            info.GetComponentInChildren<TextMesh>().text = ((int)(h*mult)).ToString();
            Hp += (int)(h*mult);
            if (Hp > Hp_Max) Hp = Hp_Max;
        }
        else
        {
            var info = Instantiate(health_indicator, transform.position, Quaternion.identity);
            info.GetComponentInChildren<TextMesh>().text = "MAX";
            Hp = Hp_Max;
        }
           
    }
    public void unlock_power(int t)
    {

        power_selected = t;
        power_time_helper = 60f;
        
            switch (t)
        {
            case 0:
                game_Controller.GetComponent<game_controller>().update_portal(1);
                water_power = true;
                break;
            case 1:
                game_Controller.GetComponent<game_controller>().update_portal(1);
                ground_power = true;
                break;
            case 2:
                game_Controller.GetComponent<game_controller>().update_portal(1);
                fire_power = true;
                break;
            case 3:
                game_Controller.GetComponent<game_controller>().update_portal(1);
                air_power = true;
                break;
        }
    }

}
