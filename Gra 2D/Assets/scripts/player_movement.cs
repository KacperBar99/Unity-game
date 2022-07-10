using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class player_movement : MonoBehaviour

{
    public GameObject game_controller;
    public bool ready = false;
    public int id = 0;
    public player_controller controller;
    public Animator animator;
    public float horizontalMove = 0;
    public float runSpeed = 40;
    bool jump = false;
    public bool crouch = false;
    public bool look_up=false;
    public bool look_down=false;
    public bool look_left = false;
    public bool look_right = false;
    public bool look_straight = true;
    [SerializeField] private Collider2D m_PhaseDisable;
    


    private void Start()
    {
        game_controller = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        

        if (game_controller.GetComponent<game_controller>().Pause == true) return;



        if (game_controller.GetComponent<game_controller>().is_map == false)
        {
            if (ready == false) return;
            //test poruszania
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            //poruszanie siê w poziomie
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            
            

            if (Input.GetButtonDown("Jump"))//skok
            {

                jump = true;
                if (controller.m_Grounded == false)
                    animator.SetBool("Jumped", true);
            }
            if (Input.GetButtonDown("Crouch"))//kucanie
            {
                crouch = true;
                

            }
            else if (crouch && Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }

            //Klawiatura

            if(game_controller.GetComponent<game_controller>().keyboard)
            {
                if (Input.GetButtonDown("Look right key"))
                {
                    look_right = true;
                    if (controller.m_FacingRight == false)
                    {

                        controller.Flip();
                    }
                }
                else if (Input.GetButtonUp("Look right key"))
                {
                    look_right = false;
                }
                if (Input.GetButtonDown("Look left key"))
                {
                    look_left = true;
                    if (controller.m_FacingRight == true)
                    {
                        controller.Flip();
                    }
                }
                else if (Input.GetButtonUp("Look left key"))
                {
                    look_left = false;
                }


                //celowanie góra dó³ i tak dalej
                if (Input.GetButtonDown("Look up key"))
                {
                    look_up = true;
                    animator.SetBool("Look up", true);
                }
                else if (Input.GetButtonUp("Look up key"))
                {
                    look_up = false;
                    animator.SetBool("Look up", false);
                }
                else if (Input.GetButtonDown("Look down key"))
                {
                    look_down = true;
                    animator.SetBool("look down", true);
                }
                else if (Input.GetButtonUp("Look down key"))
                {
                    look_down = false;
                    animator.SetBool("look down", false);
                }
                /*
                if (Input.GetButtonDown("Look straight"))
                {
                    look_straight = !look_straight;
                    animator.SetBool("Straight", look_straight);
                }*/
            }
            else
            {
                if (Input.GetAxis("Look right") > 0.1)
                {
                    look_right = true;
                    if (controller.m_FacingRight == false)
                    {

                        controller.Flip();
                    }
                }
                else
                {
                    look_right = false;
                }
                if (Input.GetAxis("Look right") < -0.1)
                {
                    look_left = true;
                    if (controller.m_FacingRight == true)
                    {
                        controller.Flip();
                    }
                }
                else look_left = false;


                if (Input.GetAxis("Look up") < -0.1)
                {
                    look_up = true;
                    animator.SetBool("Look up", true);
                }
                else
                {
                    look_up = false;
                    animator.SetBool("Look up", false);
                }
                if (Input.GetAxis("Look up") > 0.1)
                {
                    look_down = true;
                    animator.SetBool("look down", true);
                }
                else
                {
                    look_down = false;
                    animator.SetBool("look down", false);
                }
            }

            
            

            
                
            

           



            if (controller.m_FacingRight==true && look_right==true)
            {
                look_straight = false;
                animator.SetBool("Straight", false);
            }
            else if(controller.m_FacingRight==false && look_left==true)
            {
                look_straight = false;
                animator.SetBool("Straight", false);
            }
            else
            {
                look_straight = true;
                animator.SetBool("Straight", true);
            }
        }
        else return;
        
    }
   
    
    void FixedUpdate()
    {
        if (ready == false) return;
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
        animator.SetBool("Jumped", !controller.m_Grounded);
        //crouch = controller.is_Crouching;
        animator.SetBool("Crouch",controller.is_Crouching);

    }
    public void set_death()
    {
        animator.SetBool("Jumped", false);
        animator.SetBool("Straight", false);
        animator.SetBool("Look up",false);
        animator.SetBool("look down",false);
        animator.SetBool("Crouch",false);
        animator.SetBool("Alive", false);
    }
}
