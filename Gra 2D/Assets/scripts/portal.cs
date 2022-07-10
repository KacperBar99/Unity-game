using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    public int status = -1;
    public game_controller gameController;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,0);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<game_controller>();
        gameController.portal = this.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player" && status==3)
        {
            Scene_static_sync.sync_hp = gameController.player1.GetComponent<player_adventure>().Hp;
            Scene_static_sync.sync_xp= gameController.player1.GetComponent<player_adventure>().xp;
            Scene_static_sync.sync_round++;
            SceneManager.LoadScene("SinglePlayer", LoadSceneMode.Single);
        }
    }
    public void updateV()
    {
        switch(status)
        {
            case 0:
                
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b,1f/4f);
                break;
            case 1:
                
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f/2f);
                break;
            case 2:
                
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b,3f/4f);
                break;
            case 3:
                
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
                break;
        }
    }

}
