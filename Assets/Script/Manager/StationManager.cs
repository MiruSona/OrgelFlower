using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour {

    //체크할 값
    public SaveManager.Stage stage = SaveManager.Stage.YellowGarden;
    private int level = 1;

    //참조
    public Portal[] doors;

    //초기화
    void Start () {
        //스테이지 클리어 한거 있나 체크
        string data = SaveManager.LoadStage(stage);
        if (data != null && data != "")
            level = int.Parse(data);

        //문 수랑 로드한 값이랑 체크
        if (level <= doors.Length)
        {
            //레벨 만큼 문열기
            for (int i = 0; i < level; i++)
                doors[i].Open = true;
        }

        //체력 초기화
        DataManager.player_data.InitHP();
    }
}
