using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Simple2DLight;
using Define;

public class SystemManager : MonoBehaviour {

    //참조
    private PlayerData player_data;
    private LightManager light_manager;
    private FadeUI fade_ui;
    private Portal[] portals;
    private Camera main_cam;
    private ButtonBase[] buttons;

    //부활포인트
    private string revive_point = "";

    //확대축소 관련
    private bool zoom_init = false;
    private bool zoom_save = false;
    private float init_distance = 0f;
    private float zoom_current = 0f;
    private float zoom_sensitivity = 0.015f;
    private const float zoom_min = 4.0f;
    private const float zoom_max = 11.0f;
    private const float zoom_default = 7.0f;

	//초기화
	void Start () {
        player_data = DataManager.player_data;
        light_manager = LightManager.instance;
        fade_ui = FadeUI.instance;
        portals = FindObjectsOfType<Portal>();
        main_cam = Camera.main;
        buttons = FindObjectsOfType<ButtonBase>();

        //Vsync 끄기
        Application.targetFrameRate = -1;

        //화면 안꺼지게 처리
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //부활 포인트 가져오기
        revive_point = SaveManager.Load(SaveManager.revive_point_key);

        //Zoom 처리
        string zoom_save_value = SaveManager.Load(SaveManager.zoom_key);
        if (zoom_save_value != null && zoom_save_value != "")
            zoom_current = float.Parse(zoom_save_value);
        else
            zoom_current = zoom_default;
        main_cam.orthographicSize = zoom_current;

        //라이트 크기 처리
        SetGlobalLightSize();

        //업데이트 실행
        StartCoroutine(UpdateSystem());
    }

    private void Update()
    {
        //확대/축소 PC
        ZoomInOutPC();

        //확대/축소 모바일
        ZoomInOutMobile();
    }

    //업데이트
    private IEnumerator UpdateSystem()
    {
        while (true)
        {
            //뒤로가기 누르면 꺼지게
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            
            //플레이어 상태에 따라 처리
            switch (player_data.state)
            {
                case PlayerData.State.Alive:
                    Alive();
                    break;

                case PlayerData.State.Attacked:
                    Attacked();
                    break;

                case PlayerData.State.Die:
                    Die();
                    break;
            }

            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
        }
    }

    #region 상태 처리
    private void Alive()
    {
        bool portal_do = false;

        //포탈 작동중이면 일시정지
        foreach (Portal p in portals)
        {
            if (p.GetCheckPlayer())
            {
                portal_do = true;
                Time.timeScale = 0;
                break;
            }
        }

        //아니면 페이드 인
        if (!portal_do)
        {
            fade_ui.FadeIn();
            //일시정지 풀기
            Time.timeScale = 1;
        }
    }

    private void Attacked()
    {
        //일시정지
        Time.timeScale = 0;

        //페이드 아웃
        fade_ui.FadeOut();

        //끝났으면 씬 재시작 + 살았다 표시
        if (fade_ui.GetDone())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            player_data.state = PlayerData.State.Alive;
            player_data.Init();
        }
    }

    private void Die()
    {
        //일시정지
        Time.timeScale = 0;

        //페이드 아웃
        fade_ui.FadeOut();

        //끝났으면
        if (fade_ui.GetDone())
        {
            //가장 최근 부활포인트로 부활
            if(revive_point != null && revive_point != "")
                SceneManager.LoadScene(revive_point);
            //부활포인트 없으면 그냥 현재 씬으로 다시 로드
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            //살았다 표시
            player_data.state = PlayerData.State.Alive;
            player_data.InitHP();
            player_data.Init();
        }
    }
    #endregion

    #region 확대 축소
    //확대 축소 PC
    private void ZoomInOutPC()
    {
        //기본
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //카메라 처리
            zoom_current = zoom_default;
            main_cam.orthographicSize = zoom_current;

            //라이트 처리
            SetGlobalLightSize();

            //확대/축소값 저장
            SaveManager.Save(zoom_current.ToString(), SaveManager.zoom_key);
        }
        //축소
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //카메라 처리
            zoom_current = zoom_min;
            main_cam.orthographicSize = zoom_current;

            //라이트 처리
            SetGlobalLightSize();

            //확대/축소값 저장
            SaveManager.Save(zoom_current.ToString(), SaveManager.zoom_key);
        }
        //확대
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //카메라 처리
            zoom_current = zoom_max;
            main_cam.orthographicSize = zoom_current;

            //라이트 처리
            SetGlobalLightSize();

            //확대/축소값 저장
            SaveManager.Save(zoom_current.ToString(), SaveManager.zoom_key);
        }
    }

    //확대 축소 모바일
    private void ZoomInOutMobile()
    {
        //터치
        int count = Input.touchCount;

        //터치가 2개 이상 있으면
        if (count >= 2)
        {
            //터치 움직이는지 / 유지하는지 체크
            if (CheckTouchStay(0) && CheckTouchStay(1))
            {
                //각 터치 위치값
                Vector2 pos1 = Input.GetTouch(0).position;
                Vector2 pos2 = Input.GetTouch(1).position;

                //현재 거리 구하기
                float current_dist = Vector2.Distance(pos1, pos2);

                //초기값 저장
                if (!zoom_init)
                {
                    init_distance = current_dist;
                    zoom_init = true;
                    //확대/축소값 다시 저장할 수 있게
                    zoom_save = false;
                }

                //0으로 나누는거 방지
                if (zoom_sensitivity == 0f)
                    zoom_sensitivity = 0.001f;
                //확대값 = (현재거리 - 초기거리) * 민감도
                float zoom_value = (current_dist - init_distance) * zoom_sensitivity;

                //확대 축소 처리
                zoom_current = main_cam.orthographicSize;
                zoom_current -= zoom_value;

                //최대 최소 처리
                if (zoom_current < zoom_min)
                    zoom_current = zoom_min;
                else if (zoom_current > zoom_max)
                    zoom_current = zoom_max;

                //대입
                main_cam.orthographicSize = zoom_current;

                //라이트 크기 처리
                SetGlobalLightSize();
            }
        }
        //터치 없으면 초기화
        else
        {
            zoom_init = false;
            init_distance = 0f;

            //확대/축소값 저장
            if (!zoom_save)
            {
                SaveManager.Save(zoom_current.ToString(), SaveManager.zoom_key);
                zoom_save = true;
            }
        }
    }

    //터치 유지 여부 체크
    private bool CheckTouchStay(int _index)
    {
        bool send = false;

        //버튼 하나라도 작동중이면 실행 안함
        foreach (ButtonBase btn in buttons)
        {
            if (btn.pressed)
                return send;
        }

        if (Input.GetTouch(_index).phase == TouchPhase.Moved)
            send = true;

        if (Input.GetTouch(_index).phase == TouchPhase.Stationary)
            send = true;

        return send;
    }

    //라이트 크기 처리(확대/축소에 따라 라이트 크기도 변경)
    private void SetGlobalLightSize()
    {
        light_manager.SetGlobalSize(zoom_default / zoom_current);
    }
    #endregion
}
