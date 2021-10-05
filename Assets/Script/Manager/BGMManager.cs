using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour {
    
    //사운드!
    private AudioSource audio_source;
    public AudioClip Title;
    public AudioClip YellowGarden, BlueGarden, RedGarden;

    //체크할 스테이지
    private const string title_scene = "Title", credit_scene = "Credit";
    private const string central_garden_scene = "CentralGarden";
    private const string yellow_garden_scene = "YellowStation";
    private const string blue_garden_scene = "BlueStation";
    private const string red_garden_scene = "RedStation";

    //초기화
    void Start () {
        //컴포넌트 초기화
        audio_source = GetComponent<AudioSource>();

        //오브젝트 파괴 안되도록
        DontDestroyOnLoad(gameObject);

        //씬 로드마다 체크할 함수
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    //씬 로드될때 마다 체크할꺼
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //씬에 따라 BGM 실행
        switch (scene.name)
        {
            case title_scene:
            case central_garden_scene:
            case credit_scene:
                ChangeBGM(Title);
                break;
            case yellow_garden_scene: ChangeBGM(YellowGarden); break;
            case blue_garden_scene: ChangeBGM(BlueGarden); break;
            case red_garden_scene: ChangeBGM(RedGarden); break;
        }
    }

    //BGM 변경 및 실행
    private void ChangeBGM(AudioClip _clip)
    {
        if(audio_source.clip != _clip)
        {
            audio_source.clip = _clip;
            audio_source.Play();
        }
    }
}
