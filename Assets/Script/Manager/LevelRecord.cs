using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRecord : MonoBehaviour {

    //기록 할꺼
    public SaveManager.Stage stage = SaveManager.Stage.YellowGarden;
    public int level = 1;

    //컴포넌트
    private Portal portal;

    //저장 실행 여부
    private bool record = false;

	//초기화
	void Start () {
        portal = FindObjectOfType<Portal>();
        level++;
    }
	
	void Update ()
    {
        //문 열렸나 확인
        if (portal.Open)
        {
            //세이브
            if (!record)
                RecordLevel();
        }
	}

    //세이브 처리
    private void RecordLevel()
    {
        //데이터 로드
        string load_data = SaveManager.LoadStage(stage);
        int data = 0;
        if (load_data != null && load_data != "")
            data = int.Parse(load_data);
        else
            data = level;

        //현재 레벨이 로드한 데이터 보다 크거나 같으면 저장
        if (level >= data)
            SaveManager.SaveStage(level.ToString(), stage);

        record = true;
    }
}
