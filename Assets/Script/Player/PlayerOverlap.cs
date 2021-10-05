using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverlap : MonoBehaviour {

    //참조
    private PlayerData player_data;
    
    //초기화
    void Start () {
        player_data = DataManager.player_data;
    }

    //땅과 겹치면 죽게
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && !collision.usedByEffector)
        {
            player_data.SubHP();
        }
    }
}
