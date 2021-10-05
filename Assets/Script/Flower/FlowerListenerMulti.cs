using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class FlowerListenerMulti : MonoBehaviour {

    //어느 색에 반응할지 선택 -> None이면 반응 안함
    [HideInInspector]
    public FlowerColor[] CheckColors = new FlowerColor[1];
    //현재 반응한 색
    private FlowerColor current_color = FlowerColor.None;
    //엑티브 관련
    private bool active = false;
    public bool always_active = false;
    public float active_time = 5f;
    private Timer active_timer = new Timer(0, 5f);

    //초기화
    private void Start()
    {
        active_timer = new Timer(0, active_time);
    }

    private void FixedUpdate()
    {
        //활성화 됬다면 & 항상 엑티브 표시 안됬다면
        if (active && !always_active)
        {
            //일정 시간 뒤 초기화
            if (!active_timer.CheckTimer())
                active_timer.AddTimer();
            else
            {
                active_timer.InitTimer();
                //Active false로
                active = false;
                //현재 색 None으로
                current_color = FlowerColor.None;
            }
        }
    }

    //에코에 반응
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckEcho(collision.GetComponent<EchoEffect>());
    }

    //에코에 반응
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckEcho(collision.collider.GetComponent<EchoEffect>());
    }

    //반응 및 현재 반응한 색 표시
    private void CheckEcho(EchoEffect _echo)
    {
        foreach (FlowerColor color in CheckColors)
        {
            if (color != FlowerColor.None)
            {
                if (_echo != null)
                {
                    if (_echo.flower_color == color)
                    {
                        active = true;
                        current_color = color;
                        active_timer.time = 0f;
                    }
                }
            }
        }
    }

    //강제 액티브
    public void SetActive(FlowerColor _color)
    {
        active = true;
        current_color = _color;
        active_timer.time = 0f;
    }

    //엑티브 여부
    public bool GetActive()
    {
        return active;
    }

    //현재 반응한 색 표시
    public FlowerColor GetCurrentColor()
    {
        return current_color;
    }

    //디엑티브
    public void DeActive()
    {
        //엑티브 끄기
        active = false;
        //현재 색 None으로
        current_color = FlowerColor.None;

        //항상 엑티브 아니면
        if (!always_active)
        {
            //타이머 초기화
            active_timer.InitTimer();
        }
    }
}
