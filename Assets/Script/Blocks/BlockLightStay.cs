using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simple2DLight;
using Define;

[RequireComponent(typeof(FlowerListenerMulti), typeof(NormalLight))]
public class BlockLightStay : MonoBehaviour {

    //참조
    private EchoSystem echo_system;

    //컴포넌트
    private NormalLight normal_light;
    private FlowerListenerMulti listener;
    private SpriteRenderer jewel_render;

    //켜짐 여부
    public bool LightOn = false;

    //한번 실행
    private bool do_onece = false;

    //초기화
    void Start()
    {
        //참조
        echo_system = EchoSystem.instance;

        //컴포넌트
        normal_light = GetComponent<NormalLight>();
        listener = GetComponent<FlowerListenerMulti>();
        listener.CheckColors = new FlowerColor[3] { FlowerColor.Yellow, FlowerColor.Blue, FlowerColor.Red };
        listener.always_active = true;
        jewel_render = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    //업데이트
    void FixedUpdate()
    {
        //작동했으면 에코!
        if ((listener.GetActive() || LightOn) && !do_onece)
        {
            //에코
            echo_system.Echo(listener.GetCurrentColor(), transform.position, false);

            //노말 라이트 생성
            normal_light.CreateLight(transform.position);

            //보석 색 변경
            jewel_render.color = ColorUtil.GetFlowerColor(listener.GetCurrentColor());

            do_onece = true;
        }
    }
}
