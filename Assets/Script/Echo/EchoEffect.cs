using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using Simple2DLight;

[RequireComponent(typeof(FadeLightParticle))]
public class EchoEffect : MonoBehaviour {
    
    //색
    public FlowerColor flower_color = FlowerColor.None;

    //조명
    private FadeLightParticle fade_light;

    //컬리더 관련
    private CircleCollider2D circle_col;
    private bool echo_on = false;
    private bool grow = true;
    private const float col_offset = 1.25f;
    private float target_size = 0f;
    private float grow_up = 0f;
    private float grow_down = 0f;

    //파티클
    private ParticleSystem particle;

    //사운드
    private AudioSource audio_source;
    public AudioClip SFX = null;

    //파괴 여부
    public bool DestroyEnd = false;

    //초기화
    private void Awake()
    {
        //컴포넌트
        fade_light = GetComponent<FadeLightParticle>();
        circle_col = GetComponent<CircleCollider2D>();
        particle = GetComponent<ParticleSystem>();
        audio_source = GetComponent<AudioSource>();
    }

    //매번 초기화
    private void Init()
    {
        //컬리더 크기 0으로
        if(circle_col != null)
            circle_col.radius = 0f;

        //컬리더 관련값 초기화
        target_size = fade_light.light_size * col_offset;
        grow_up = target_size / fade_light.light_grow_up_time * Time.fixedDeltaTime;
        grow_down = target_size / fade_light.light_grow_down_time * Time.fixedDeltaTime;

        //다시 커지게
        grow = true;
    }

    //업데이트
    private void FixedUpdate()
    {
        //컬리더 커지기
        if (echo_on)
        {
            if (circle_col != null)
                GrowCollider();
        }        
    }

    //컬리더 크기 조절
    private void GrowCollider()
    {
        //커지는 중
        if (grow)
        {
            //다 커질때까지++
            if (circle_col.radius + grow_up < target_size)
                circle_col.radius += grow_up;
            else
                grow = false;
        }
        //작아지는 중
        else
        {
            //다 작아질때까지--
            if (circle_col.radius - grow_down > 0)
                circle_col.radius -= grow_down;
            //다 작아지면 0으로 처리 + 파티클 종료됬으면 false
            else
            {
                circle_col.radius = 0;
                if (!particle.isPlaying)
                {
                    //파괴여부에 따라 false / 파괴
                    if (!DestroyEnd)
                        gameObject.SetActive(false);
                    else
                        Destroy(gameObject);
                }
                    
            }
        }
    }

    //에코!
    public void Echo(Vector3 _position, bool _col_onoff)
    {
        //라이트 초기화
        fade_light.Init();
        //초기화
        Init();

        //이동
        transform.position = _position;
        //켜기
        gameObject.SetActive(true);
       
        //컬리더 실행 여부
        if (_col_onoff)
            circle_col.enabled = true;
        else
            circle_col.enabled = false;

        //사운드
        if (SFX != null)
            audio_source.PlayOneShot(SFX);

        //빛나기
        fade_light.Lighting();

        echo_on = true;
        //파티클 실행
        particle.Play();
    }

    //크기 조절도
    public void Echo(Vector3 _position, float _size, bool _col_onoff)
    {
        //라이트 초기화
        fade_light.Init(_size);
        //초기화
        Init();

        //이동
        transform.position = _position;
        //켜기
        gameObject.SetActive(true);
        
        //컬리더 실행 여부
        if (_col_onoff)
            circle_col.enabled = true;
        else
            circle_col.enabled = false;

        //사운드
        if (SFX != null)
            audio_source.PlayOneShot(SFX);
        
        //빛나기
        fade_light.Lighting();

        echo_on = true;
        //파티클 실행
        particle.Play();
    }

    //전부 조절
    public void Echo(Vector3 _position, float _size, float _grow_up_time, float _grow_down_time, bool _col_onoff)
    {
        //라이트 초기화
        fade_light.Init(_size, _grow_up_time, _grow_down_time);
        //초기화
        Init();

        //이동
        transform.position = _position;
        //켜기
        gameObject.SetActive(true);

        //컬리더 실행 여부
        if (_col_onoff)
            circle_col.enabled = true;
        else
            circle_col.enabled = false;

        //사운드
        if (SFX != null)
            audio_source.PlayOneShot(SFX);

        
        //빛나기
        fade_light.Lighting();

        echo_on = true;
        //파티클 실행
        particle.Play();
    }
}
