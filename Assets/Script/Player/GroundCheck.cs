using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
    
    //참조
    private PlayerData player_data;
    private Rigidbody2D player_rb2d;

    //체크 오프셋
    private const float offset = 0.02f;

    //초기화
    void Start () {
        player_data = DataManager.player_data;
        player_rb2d = MainPlayer.instance.GetComponent<Rigidbody2D>();
    }

    //땅에 닿았나 표시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (!player_data.ground)
            {
                float value = player_rb2d.velocity.y;
                if (-offset <= value && value <= offset)
                    player_data.ground = true;
            }
        }                      
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (!player_data.ground)
            {
                float value = player_rb2d.velocity.y;
                if (-offset <= value && value <= offset)
                    player_data.ground = true;
            }
        }
    }

    //땅에 떨어졌나 표시
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (player_data.ground)
                player_data.ground = false;
        } 
    }
}
