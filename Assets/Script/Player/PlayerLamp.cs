using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simple2DLight;
using Define;

public class PlayerLamp : MonoBehaviour {

    //참조
    private PlayerData player_data;
    private EchoSystem echo_system;
    private Transform lamp_anchor;
    private Transform line_end;

    //컴포넌트
    private NormalLight normal_light;
    private SpriteRenderer flower_render;
    private LineRenderer line_render;

    //에코
    private Timer echo_delay = new Timer(1f, 1f);

    //초기화
    void Start () {
        //참조
        player_data = DataManager.player_data;
        echo_system = EchoSystem.instance;
        lamp_anchor = GetComponent<SpringJoint2D>().connectedBody.transform;
        line_end = transform.GetChild(1);

        //컴포넌트
        normal_light = GetComponent<NormalLight>();
        normal_light.CreateLight(transform.position);
        flower_render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        line_render = GetComponent<LineRenderer>();
        line_render.sortingLayerName = "Player";
        line_render.sortingOrder = -1;
    }

    //업데이트
    void FixedUpdate () {

        //조명 위치 이동
        normal_light.MoveLight(transform.position);
        //조명 선 그리기
        Vector3[] pos = { line_end.position, lamp_anchor.position };
        line_render.SetPositions(pos);

        //스프라이트 처리
        ChangeFlowerSprite();
        //에코 처리
        Echo();
    }

    //에코
    private void Echo()
    {
        //꽃 받아오기
        FlowerData flower = player_data.flower_current;

        //현재 꽃이 있다면
        if(flower != null)
        {
            //타이머 체크
            if (echo_delay.AutoTimer())
            {
                //수명이 있다면 에코!
                if (flower.CheckLife())
                {
                    //에코!
                    echo_system.Echo(flower.flower_color, transform.position, true);
                    //수명 감소
                    flower.SubLife();
                }
                else
                {
                    //없으면 현재 꽃 없다 표시
                    player_data.SelectFlower(FlowerColor.None);
                    //딜레이 초기화(누르자 마자 빛나게)
                    InitEchoTimer();
                }
            }
        }
    }

    //꽃 스프라이트 처리
    private void ChangeFlowerSprite()
    {
        //꽃 받아오기
        FlowerData flower = player_data.flower_current;

        //현재 꽃이 있다면
        if (flower != null)
        {
            //랜더러 키고
            if (!flower_render.enabled)
                flower_render.enabled = true;
            //꽃잎 받아와서 변경
            flower_render.sprite = flower.GetLeafSpriteByLife();
        }
        //없으면 랜더러 끄기
        else
            flower_render.enabled = false;
    }

    //에코 타이머 초기화
    public void InitEchoTimer()
    {
        echo_delay.time = echo_delay.limit;
    }
}
