using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

    //참조
    private PlayerData player_data;

    //컴포넌트
    private Image image;

	//초기화
	void Start () {
        player_data = DataManager.player_data;
        image = GetComponent<Image>();
    }
	
	//UI업데이트
	void Update () {
        image.fillAmount = player_data.hp / player_data.hp_max;
    }
}
