using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    //참조
    private PlayerData player_data;

    //초기화
    void Start()
    {
        player_data = DataManager.player_data;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어면 HP--
        if (collision.CompareTag("Player"))
            player_data.SubHP();
    }
}
