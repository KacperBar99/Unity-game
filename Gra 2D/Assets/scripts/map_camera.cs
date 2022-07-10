using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_camera : MonoBehaviour
{
    public GameObject gameController;
    public float camera_Speed;
    public float camera_smoothing;
    public Vector3 m_Velocity = Vector3.zero;
    public Rigidbody2D m_body;
    public GameObject player_location;

    private void Start()
    {
        
        gameController = GameObject.FindGameObjectWithTag("GameController");
        
        StartPosition();

    }
    public void StartPosition()
    {
        transform.position = new Vector3(0, 1000, -10);
    }
    public void Reset_map()
    {
        if(player_location!=null)
        {
            transform.position = new Vector3(player_location.transform.position.x, player_location.transform.position.y, -10);
        }
    }
    private void Update()
    {
        if (gameController.GetComponent<game_controller>().Pause == true) return;

        if(player_location==null) player_location = GameObject.FindGameObjectWithTag("player_location");

        if (gameController.GetComponent<game_controller>().is_map == true)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal") * camera_Speed * Time.fixedDeltaTime;
            float verticalMove = Input.GetAxisRaw("Vertical") * camera_Speed * Time.fixedDeltaTime;
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f, verticalMove * 10f);

            m_body.velocity= Vector3.SmoothDamp(m_body.velocity, targetVelocity, ref m_Velocity, camera_smoothing);


            if(Input.GetButton("Zoom map"))
            {
                
                if (gameObject.GetComponent<Camera>().orthographicSize > 1) gameObject.GetComponent<Camera>().orthographicSize-=1f*Time.deltaTime;
                else gameObject.GetComponent<Camera>().orthographicSize = 1;

            }
            else if(Input.GetButton("Unzoom map"))
            {
               
                gameObject.GetComponent<Camera>().orthographicSize+=1f*Time.deltaTime;
            }
            if(Input.GetButtonDown("Center Map"))
            {
                StartPosition();
            }
        }
    }
}
