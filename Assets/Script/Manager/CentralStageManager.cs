using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralStageManager : MonoBehaviour {

    //체크할 값
    private const int blue_open = 4;
    private const int red_open = 3;
    private const int exit_open = 4;

    //문
    public Portal yellow_door;
    public Portal blue_door;
    public Portal red_door;
    public Portal exit_door;

    //초기화
    void Start()
    {
        //노란 문은 열기
        yellow_door.Open = true;

        //스테이지 클리어 한거 있나 체크
        int yellow_stage = GetStage(SaveManager.Stage.YellowGarden);
        int blue_stage = GetStage(SaveManager.Stage.BlueGarden);
        int red_stage = GetStage(SaveManager.Stage.RedGarden);

        blue_door.Open = (yellow_stage == blue_open);
        red_door.Open = (blue_stage == red_open);
        exit_door.Open = (red_stage == exit_open);

        //체력 초기화
        DataManager.player_data.InitHP();
    }

    //데이터 받아오기
    private int GetStage(SaveManager.Stage _stage)
    {
        int result = -1;
        string data = SaveManager.LoadStage(_stage);
        if (data != null && data != "")
            result = int.Parse(data);

        return result;
    }
}
