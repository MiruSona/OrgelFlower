using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using Simple2DLight;

[RequireComponent(typeof(FlowerListenerMulti), typeof(NormalLight))]
public class Goal : MonoBehaviour {

    //참조
    private EchoEffect echo;

    //컴포넌트
    private FlowerListenerMulti listener;
    private NormalLight normal_light;

    //한번만 작동하게
    private bool do_onece = false;

	//초기화
	void Start () {
        //참조
        echo = Resources.Load<EchoEffect>("Prefab/Echo/GoalEcho");

        //컴포넌트
        listener = GetComponent<FlowerListenerMulti>();
        listener.CheckColors = new FlowerColor[3] { FlowerColor.Yellow, FlowerColor.Blue, FlowerColor.Red };
        normal_light = GetComponent<NormalLight>();
    }
	
	//업데이트
	void FixedUpdate () {

        //작동했으면 에코!
        if (listener.GetActive() && !do_onece)
        {
            //이펙트 생성
            EchoEffect effect = Instantiate(echo);
            effect.Echo(transform.position, true);

            //노말 라이트 생성
            normal_light.CreateLight(transform.position);
            do_onece = true;
        }
	}
}
