using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevivePointRecord : MonoBehaviour {

    //키 귀찮으니 저장
    private string key = "";

	//저장!
	void Start () {
        //키 가져오기
        key = SaveManager.revive_point_key;

        //현재 씬을 부활포인트로 저장
        string revive_scene = SaveManager.Load(key);
        string current_scene = SceneManager.GetActiveScene().name;
        
        if (revive_scene != null)
        {
            //저장된 부활 포인트와 다르면 저장
            if (revive_scene != current_scene)
                SaveManager.Save(current_scene, key);
        }
        //저장된거 없으면 그냥 저장
        else
            SaveManager.Save(current_scene, key);
    }
}
