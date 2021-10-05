using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Define;

public class FadeUI : SingleTon<FadeUI> {

    //컴포넌트
    private Image fade_ui;

    //변화값
    public float fade_in_delta = 0.03f;
    public float fade_out_delta = 0.05f;
    private float clamp = 0.0f;

    //bool 값
    private bool fade_onoff = false;        //fade 여부
    private bool fade_in = false;           //Fade In/Out 구분(true = fade in)
    private bool done = false;              //끝났는지 여부

    //시간값
    private const float delta_time = 0.02f;
    
    //초기화
    private void Awake () {
        fade_ui = GetComponent<Image>();
        
        Color color = fade_ui.color;
        color.a = 1.0f;
        fade_ui.color = color;
    }

    //Fade 갱신
    private IEnumerator FadeUpdate()
    {
        while (fade_onoff)
        {
            //페이드 인 여부
            if (fade_in)
                FadeAlpha(0.0f, fade_in_delta);
            else
                FadeAlpha(1.0f, fade_out_delta);

            //끝났으면 페이드도 종료
            if (done)
                fade_onoff = false;

            yield return new WaitForSecondsRealtime(delta_time);
        }
    }

    //페이드
    private void FadeAlpha(float _target_alpha, float _fade_delta)
    {
        //Clamp값 구하기
        clamp = _fade_delta / 2.0f;

        Color color = fade_ui.color;
        float alpha = color.a;
        //러프
        alpha = Mathf.Lerp(alpha, _target_alpha, _fade_delta);
        
        //클램프
        if (alpha < clamp)
            alpha = 0f;
        if (alpha > 1.0f - clamp)
            alpha = 1.0f;

        //알파값 적용
        color.a = alpha;

        //색 적용
        fade_ui.color = color;

        //완료 여부
        done = (alpha == _target_alpha);
    }
    
    //페이드인    
    public void FadeIn()
    {
        //한번만 실행하게
        if (!fade_in)
        {
            done = false;
            fade_onoff = true;
            fade_in = true;

            //코루틴 시작
            StartCoroutine(FadeUpdate());
        }
    }

    //페이드 아웃
    public void FadeOut()
    {
        //한번만 실행하게
        if (fade_in)
        {
            done = false;
            fade_onoff = true;
            fade_in = false;

            //코루틴 시작
            StartCoroutine(FadeUpdate());
        }
    }

    //완료 여부
    public bool GetDone()
    {
        return done;
    }
}
