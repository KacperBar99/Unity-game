using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using System;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
public class game_controller : MonoBehaviour
{

    public GameObject guide;
    public Sprite[] map_sprites; 

    public GameObject settings;
    public TMP_Text score_result;
    public TMP_Text place_result;
    public float boss_damage_ration;
   
    public float boss_damage_timer = 1f;
   public float boss_damage_timer_helper = 0f;


    public GameObject Boss_bar;
    public Slider boss_slider_2;
    public Slider boss_slider;

    public GameObject Legend;
    public GameObject portal;
    public GameObject power_fill;
    public Slider power_bar;

    public bool keyboard=true;
    public TMP_InputField input_field;
    public GameObject nickname_setup;
    public GameObject input;

    public Transform mover;
    public Gradient health_gradient;
    public GameObject hp_fill;


   
    public TMP_Text level_info;
    public GameObject HUD;
    public Slider Hp_bar;
    public Slider Xp_bar;
    public GameObject V_Cam;
    public GameObject activeRoom1;
    public GameObject activeRoom2;
    public GameObject loadingscreen;
    public Slider loading_bar;

    public GameObject pause_menu;
    
    public bool is_map=false;
    public Camera main;
    public Camera main2;
    public Camera map;
    public bool Pause = false;
    public GameObject level_generator;
    public GameObject room_map;
    public GameObject player_location;

    public GameObject player1;
    public GameObject player2;
    bool pre_pause_boss=false;

    //tymczasowe potem usunac
    public void boss_health(bool v)
    {
       // Debug.Log(v);
       if(Boss_bar!=null)
        {
            Boss_bar.SetActive(v);
            if (Boss_bar.activeSelf == true)
                boss_slider_2.value = 1f;
        }
        
    }

    float time;
    public void set_keyboard(bool k)
    {
        keyboard = k;
    }

    private void Start()
    {
        Boss_bar.SetActive(false);
        time = Time.time;
        nickname_setup.SetActive(false);
        Legend.SetActive(false);
        HUD.SetActive(false);
        pause_menu.SetActive(false);
        loadingscreen.SetActive(true);
        loading_bar.value = 0;

        main.enabled = true;
        if (main2 != null) main2.enabled = true;
        map.enabled = false;
        string line1;
        string line2;
        string line3;
        string line4;
        using (StreamReader reader = new StreamReader(@"settings.settings"))
        {
            line1 = reader.ReadLine();
            line2 = reader.ReadLine();
            line3 = reader.ReadLine();
            line4 = reader.ReadLine();
        }
        keyboard = Convert.ToBoolean(line4);
        power_fill.GetComponent<Image>().color = Color.black;
    }
    public void Get_Name()
    {

        
        set_place();
        
        score_result.text= (player1.GetComponent<player_adventure>().xp + 100 * Scene_static_sync.sync_round+Scene_static_sync.bonus).ToString();
        nickname_setup.SetActive(true);
        EventSystem.current.SetSelectedGameObject(input);
    }
    void set_place()
    {

        int []score = new int[10];
        
        int current_score = player1.GetComponent<player_adventure>().xp + 100 * Scene_static_sync.sync_round + Scene_static_sync.bonus;
        int i = 0;
        using (StreamReader reader = new StreamReader(@"score.score"))
        {
            string line1;
            string line2;
            string line3;
            while ((line1 = reader.ReadLine()) != null && i < 10)
            {
                line2 = reader.ReadLine();
                line3 = reader.ReadLine();
                
                score[i] = Convert.ToInt32(line2);
                
                i++;

            }

        }
        i = 0;
        for(int j=0;j<10;j++)
        {
            if (current_score > score[j]) i++;
        }
        if (i != 0)
            place_result.text = (11 - i).ToString();
        else place_result.text = "none";
    }
    public void End_Game()
    {
       
        if (input_field.GetComponent<TMP_InputField>().text == null || input_field.GetComponent<TMP_InputField>().text=="")
        {
            
            return; 
        }
        //float tmp_time = Time.time - time;
        string[] nickname = new string[10];
        string[] score = new string[10];
        string[] date = new string[10];

        int current_score = player1.GetComponent<player_adventure>().xp+100*Scene_static_sync.sync_round+Scene_static_sync.bonus;

        int i = 0;
        using (StreamReader reader = new StreamReader(@"score.score"))
        {
            string line1;
            string line2;
            string line3;
            while ((line1 = reader.ReadLine()) != null && i < 10)
            {
                line2 = reader.ReadLine();
                line3 = reader.ReadLine();
                nickname[i] = line1;
                score[i] = line2;
                date[i] = line3;
                i++;

            }

        }

        List<string> nickname_ = new List<string>();
        List<string> score_ = new List<string>();
        List<string> date_ = new List<string>();
        bool set = false;
        for (int j = 0; j < 10; j++)
        {
            if (string.IsNullOrEmpty(score[j]) == true)
            {
                nickname_.Add(input_field.GetComponent<TMP_InputField>().text);
                score_.Add(current_score.ToString());
                date_.Add(System.DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
                set = true;

            }

            if (set == false && string.IsNullOrEmpty(score[j]) == false && Convert.ToDouble(score[j]) < current_score)
            {
                nickname_.Add(input_field.GetComponent<TMP_InputField>().text);
                score_.Add(current_score.ToString());
                date_.Add(System.DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
                set = true;
                nickname_.Add(nickname[j]);
                score_.Add(score[j]);
                date_.Add(date[j]);
            }
            else
            {
                nickname_.Add(nickname[j]);
                score_.Add(score[j]);
                date_.Add(date[j]);
            }


        }






        using (StreamWriter writer = new StreamWriter(@"score.score"))
        {
            for (int j = 0; j < 10; j++)
            {

                writer.WriteLine(nickname_[j]);
                writer.WriteLine(score_[j]);
                writer.WriteLine(date_[j]);
            }
        }
        SceneManager.LoadScene("Main_menu", LoadSceneMode.Single);
    }
    public void exit_to_menu()
    {
        player1.GetComponent<player_movement>().ready = false;
        Pausing();
        Get_Name();        
    }
    public void exit_game()
    {
        Application.Quit();
    }
    public void Pausing()
    {
        if (nickname_setup.activeSelf == true) return;
        if (level_generator.GetComponent<Level_generating>().done_generating == false) return;
        if (Pause == false)
        {
            pause_menu.GetComponent<pause_menu>().set_menu();
            pause_menu.SetActive(true);

            Time.timeScale = 0f;
            Pause = true;

            pre_pause_boss = Boss_bar.activeSelf;
            Boss_bar.SetActive(false);
        }
        else if (Pause == true)
        {
            settings.GetComponent<settings>().go_back_game();
            guide.GetComponent<guide>().go_back();
            pause_menu.SetActive(false);
            guide.SetActive(false);
            Time.timeScale = 1f;
            Pause = false;
            if (pre_pause_boss == true)
            {
                Boss_bar.SetActive(true);
                pre_pause_boss = false;
            }
        }
    }
    private void Update()
    {
        if(loadingscreen.activeSelf==true)
        {
            float tmp1 = level_generator.GetComponent<Level_generating>().current_rooms;
            float tmp2 = level_generator.GetComponent<Level_generating>().desired_Rooms;
            float tmp3 = level_generator.GetComponent<Level_generating>().secod_load;
            loading_bar.value = tmp1 /tmp2-.2f+tmp3;
            if (level_generator.GetComponent<Level_generating>().done_generating == true)
            { 
                loadingscreen.SetActive(false);
                HUD.SetActive(true);
            }
        }
        activeRoom1 = player1.GetComponent<player_adventure>().active_Room;
        /*
        foreach(GameObject room in level_generator.GetComponent<Level_generating>().Rooms)
        {
            if(player1.GetComponent<player_adventure>().grid_position_x==room.GetComponent<Room_Controller>().grid_position_x && player1.GetComponent<player_adventure>().grid_position_y==room.GetComponent<Room_Controller>().grid_position_y)
            {
                activeRoom1 = room;
                break;
            }
        }*/
        if(activeRoom1!=null)
        V_Cam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = activeRoom1.GetComponent<Room_Controller>().bound;
        
        if(loadingscreen.activeSelf==false && Application.isFocused==false && Pause==false)
        {
            Pausing();
        }


        if(Input.GetButtonDown("Pause"))
        {
            if(is_map==true)
            {
                Legend.SetActive(false);
                HUD.SetActive(true);
                main.enabled = true;
                if (main2 != null) main2.enabled = true;
                map.enabled = false;
                is_map = false;
            }
            else
            Pausing();
        }
        if (Pause == true) return;


        if(Input.GetButtonDown("Map"))
        {
            if (player1.GetComponent<player_movement>().ready == false) return;
            map.GetComponent<map_camera>().Reset_map();

            if(is_map==false)
            {
                Legend.SetActive(true);
                HUD.SetActive(false);
                main.enabled = false;
                if (main2 != null) main2.enabled = false;
                
                map.enabled = true;
                is_map = true;
            }
            else
            {
                Legend.SetActive(false);
                HUD.SetActive(true);
                main.enabled = true;
                if (main2 != null) main2.enabled = true;
                map.enabled = false;
                is_map = false;
            }
        }
    }
    private void FixedUpdate()
    {


        
        player_adventure tmp = player1.GetComponent<player_adventure>();

        float tmp1 = tmp.Hp;
        float tmp2 = tmp.Hp_Max;
        Hp_bar.value = tmp1 /tmp2;

        hp_fill.GetComponent<Image>().color = health_gradient.Evaluate(Hp_bar.normalizedValue);
        tmp1 = tmp.xp_needed - tmp.xp_needed_last;
        tmp2 = tmp.xp - tmp.xp_needed_last;
        Xp_bar.value = tmp2 / tmp1;
        
        if(tmp.character_level<10)
        {
            level_info.text = "0" + tmp.character_level.ToString();
        }
        else level_info.text = tmp.character_level.ToString();

        switch (tmp.power_selected)
        {
            case 0:power_fill.GetComponent<Image>().color = Color.blue;
                break;
            case 1:power_fill.GetComponent<Image>().color = Color.yellow;
                break;
            case 2:power_fill.GetComponent<Image>().color = Color.red;
                break;
            case 3:power_fill.GetComponent<Image>().color = Color.gray;
                break;
            default:power_fill.GetComponent<Image>().color = Color.black;
                break;
        }
        tmp1 = tmp.power_time;
        tmp2 = tmp.power_time_helper;
        power_bar.value = tmp2 / tmp1;

        if(boss_slider.value <= boss_slider_2.value && Boss_bar.activeSelf==true)
        {

            boss_damage_ration = boss_slider_2.value - boss_slider.value+0.01f;
            boss_damage_timer_helper += Time.deltaTime;
            if (boss_damage_timer_helper >= boss_damage_timer)
            {
                boss_slider_2.value -= boss_damage_ration * Time.deltaTime;
            }
           
            

            
        }
        

    }
    public void Create_Map()
    {
        var instpl1=new Vector3(player1.GetComponent<player_adventure>().grid_position_x, player1.GetComponent<player_adventure>().grid_position_y + 1000, 0);
        var pos1=Instantiate(player_location, instpl1, Quaternion.identity);
        pos1.GetComponent<player_location>().player = player1;
        if(player2!=null)
        {
            var instpl2= new Vector3(player2.GetComponent<player_adventure>().grid_position_x, player2.GetComponent<player_adventure>().grid_position_y + 1000, 0);
            var pos2 = Instantiate(player_location, instpl2, Quaternion.identity);
            pos2.GetComponent<player_location>().player = player2;
        }

        foreach (GameObject level in level_generator.GetComponent<Level_generating>().Rooms)
        {
            //Debug.Log(level.GetComponent<Room_Controller>().grid_position_x+" "+level.GetComponent<Room_Controller>().grid_position_y);
            var tmp = new Vector3(level.GetComponent<Room_Controller>().grid_position_x,level.GetComponent<Room_Controller>().grid_position_y,0);
            var map_object= Instantiate(room_map,tmp,Quaternion.identity);
            
            map_object.transform.position = new Vector3(map_object.transform.position.x, map_object.transform.position.y + 1000, 0);
            map_object.GetComponent<map_tile>().room = level;
            if(level.GetComponent<Room_Controller>().is_destination==true)
            {
                map_object.GetComponent<Change_Sprite>().new_Sprite = map_sprites[level.GetComponent<Room_Controller>().type_id - 1];
                map_object.GetComponent<Change_Sprite>().change_Sprite();
            }
        }
    }
    public void update_portal(int s)
    {
        portal.GetComponent<portal>().status += s;
        portal.GetComponentInChildren<portal>().updateV();
    }
    public void Share()
    {

        string timeNow = DateTime.Now.ToString("dd-MMMM-yyyy HHmmss");
        ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory() + "/ScreenShots/ScreenShot " + timeNow + ".png");

        Invoke("open_URL", 1f);
    }
    public void open_URL()
    {
        Application.OpenURL("https://www.facebook.com/");
        Application.OpenURL("file://" + Directory.GetCurrentDirectory() + "/ScreenShots");
    }
}
