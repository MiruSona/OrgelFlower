using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager {

    //키 관련
    //스테이지
    public enum Stage
    {
        YellowGarden, BlueGarden, RedGarden
    }
    //스테이지 키 처리
    public static string GetStageKey(Stage _stage)
    {
        string key = "";
        switch (_stage)
        {
            case Stage.YellowGarden: key = "Y!garden*K"; break;
            case Stage.BlueGarden: key = "B!garden*K"; break;
            case Stage.RedGarden: key = "R!garden*K"; break;
        }

        return key;
    }

    //부활 포인트 키
    public static string revive_point_key = "R!vive*K";

    //확대 축소값 저장 키
    public static string zoom_key = "Z!oom*K";

    //세이브
    public static void Save(string _value, string _key)
    {
        PlayerPrefs.SetString(_key, _value);
    }
    public static void SaveStage(string _value, Stage _stage)
    {
        string key = GetStageKey(_stage);
        Save(_value, key);
    }

    //로드
    public static string Load(string _key)
    {
        return PlayerPrefs.GetString(_key);
    }
    public static string LoadStage(Stage _stage)
    {
        string key = GetStageKey(_stage);
        string result = Load(key);

        return result;
    }
}
