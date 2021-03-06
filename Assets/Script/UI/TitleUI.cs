using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Simple2DLight;
using Define;

public class TitleUI : MonoBehaviour {
    
    //컴포넌트
    private NormalLight normal_light;
    private FadeLight fade_light;
    public Text BlinkText;

    //타이머
    public float delay = 2.0f;
    private Timer delay_timer = new Timer(0f, 1f);

    //색 변경 관련
    public float BlinkDelta = 0.01f;
    private bool alpha_up = false;

    //초기화
    private void Start()
    {
        normal_light = GetComponent<NormalLight>();
        fade_light = GetComponent<FadeLight>();

        //화면 안꺼지게 처리
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //노말 라이트 생성
        normal_light.CreateLight(Vector3.zero);

        //타이머
        delay_timer = new Timer(delay, delay);
    }

    //업데이트
    private void Update()
    {
        //끄기
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //일정 주기마다 빛나기
        if (delay_timer.AutoTimer())
            fade_light.Lighting();

        //텍스트 깜빡이기
        BlinkUI();
    }

    //로드
    public void LoadScene()
    {
        //바로 다음 씬 열기
        SceneManager.LoadScene(1);
    }

    //텍스트 깜빡이기
    private void BlinkUI()
    {
        Color color = BlinkText.color;
        if (alpha_up)
        {
            if (color.a < 0.95f)
                color.a = Mathf.Lerp(color.a, 1.0f, BlinkDelta);
            else
            {
                color.a = 1.0f;
                alpha_up = false;
            }
        }
        else
        {
            if (color.a > 0.05f)
                color.a = Mathf.Lerp(color.a, 0.0f, BlinkDelta);
            else
            {
                color.a = 0.0f;
                alpha_up = true;
            }
        }

        BlinkText.color = color;
    }
}
