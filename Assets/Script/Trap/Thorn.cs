using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour {

    //참조
    private PlayerData player_data;

	//초기화
	void Start () {
        player_data = DataManager.player_data;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어면 HP--
        if (collision.collider.CompareTag("Player"))
            player_data.SubHP();
    }
}
