using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class BlockPower2 : MonoBehaviour {
    //참조
    private EchoSystem echo_system;
    private FlowerListener start_listener, end_listener;
    private SpriteRenderer start_render, end_render;
    private Transform[] points;
    private SpriteRenderer[] points_render;

    //컴포넌트
    private LineRenderer line_render;

    //색
    private FlowerColor start_color = FlowerColor.None;
    private FlowerColor end_color = FlowerColor.None;

    //초기화 여부
    private bool init = false;

    //포인트 관련
    private int point_index = 0;        //포인트 인덱스
    private int point_start = 0;        //시작 인덱스

    //에코 관련
    private const float mini_echo_size = 1.5f;
    private const float mini_grow_up = 0.7f;
    private const float mini_grow_down = 1.0f;

    //딜레이
    private Timer echo_delay = new Timer(0, 0.2f);
    private Timer deactive_delay = new Timer(0, 0.3f);

    //초기화
    void Start()
    {
        //참조
        echo_system = EchoSystem.instance;

        //리스너
        FlowerListener[] listeners = GetComponentsInChildren<FlowerListener>();
        foreach (FlowerListener fl in listeners)
        {
            //항상 작동
            fl.always_active = true;

            //색 변경
            SpriteRenderer render = fl.GetComponent<SpriteRenderer>();
            render.color = ColorUtil.GetFlowerColor(fl.CheckColor);

            //초기화
            if (fl.gameObject.name == "StartBlock")
                start_listener = fl;
            else if (fl.gameObject.name == "EndBlock")
                end_listener = fl;
        }

        //상징 색
        start_render = start_listener.transform.GetChild(0).GetComponent<SpriteRenderer>();
        end_render = end_listener.transform.GetChild(0).GetComponent<SpriteRenderer>();

        //포인트
        points_render = GetComponentInChildren<CopyBlocks>().GetComponentsInChildren<SpriteRenderer>();
        points = new Transform[points_render.Length];
        for (int i = 0; i < points.Length; i++)
            points[i] = points_render[i].transform;

        //블럭 옮기기
        start_listener.transform.position = points[0].position;
        end_listener.transform.position = points[points.Length - 1].position;

        //컴포넌트
        line_render = GetComponent<LineRenderer>();
        line_render.sortingLayerName = "Ground";
        line_render.sortingOrder = -1;
        line_render.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
            line_render.SetPosition(i, points[i].position);
    }

    //업데이트
    void FixedUpdate()
    {
        //리스너 둘 중 하나라도 활성화 하면 실행
        if (start_listener.GetActive() || end_listener.GetActive())
        {
            CheckActiveAndInit();
            EchoPoints();
        }
        //아니면 초기화 가능하다 표시
        else
        {
            init = false;
            //상징 색 원래대로 변경
            start_render.color = ColorUtil.GetFlowerColor(FlowerColor.None);
            end_render.color = ColorUtil.GetFlowerColor(FlowerColor.None);
        }
    }

    //엑티브 체크 및 초기화
    private void CheckActiveAndInit()
    {
        //초기화 했는지 여부
        if (!init)
        {
            //시작 부분 인식 시
            if (start_listener.GetActive())
            {
                //시작 인덱스 초기화
                point_start = 0;
                point_index = point_start + 1;

                //색 가져오기
                start_color = start_listener.CheckColor;
                end_color = end_listener.CheckColor;

                //강제 엑티브
                end_listener.SetActive();

                //시작 에코
                echo_system.Echo(start_color, points[point_start].position, true);
            }
            //끝 부분 인식 시
            else if (end_listener.GetActive())
            {
                //시작 인덱스 초기화
                point_start = points.Length - 1;
                point_index = point_start - 1;

                //시작 색 가져오기
                start_color = end_listener.CheckColor;
                end_color = start_listener.CheckColor;

                //강제 엑티브
                start_listener.SetActive();

                //시작 에코
                echo_system.Echo(start_color, points[point_start].position, true);
            }

            //상징 색 변경
            start_render.color = ColorUtil.GetFlowerColor(start_listener.CheckColor);
            end_render.color = ColorUtil.GetFlowerColor(end_listener.CheckColor);

            //초기화 했다 표시
            init = true;
        }
    }

    //각 포인트 마다 빛나기
    private void EchoPoints()
    {
        //0부터 시작일 경우
        if (point_start == 0)
        {
            //에코 딜레이
            if (echo_delay.AutoTimer())
            {
                //마지막 전까지는 에코(반응X)
                if (point_index < points.Length - 1)
                    echo_system.Echo(
                        start_color,
                        points[point_index].position,
                        mini_echo_size,
                        mini_grow_up,
                        mini_grow_down,
                        false);
                //마지막꺼는 효과있는 에코
                else if (point_index == points.Length - 1)
                    echo_system.Echo(end_color, points[point_index].position, true);

                //포인트 색 처리
                if (point_index <= points.Length - 1)
                {
                    points_render[point_index].color = ColorUtil.GetFlowerColor(start_color);
                    points_render[point_index - 1].color = ColorUtil.GetFlowerColor(FlowerColor.None);
                }

                if (point_index <= points.Length - 1)
                    point_index++;
            }

            //마지막 에코도 끝나면
            if (point_index == points.Length)
            {
                //딜레이 후 리스너 디엑티브
                if (deactive_delay.AutoTimer())
                {
                    start_listener.DeActive();
                    end_listener.DeActive();
                }
            }
        }
        //끝부터 시작일 경우
        else if (point_start == points.Length - 1)
        {
            //에코 딜레이
            if (echo_delay.AutoTimer())
            {
                //마지막 전까지는 에코(반응X)
                if (point_index > 0)
                    echo_system.Echo(
                        start_color,
                        points[point_index].position,
                        mini_echo_size,
                        mini_grow_up,
                        mini_grow_down,
                        false);
                //마지막꺼는 효과있는 에코
                else if (point_index == 0)
                    echo_system.Echo(end_color, points[point_index].position, true);

                //포인트 색 처리
                if (point_index >= 0)
                {
                    points_render[point_index].color = ColorUtil.GetFlowerColor(start_color);
                    points_render[point_index + 1].color = ColorUtil.GetFlowerColor(FlowerColor.None);
                }

                if (point_index >= 0)
                    point_index--;
            }

            //마지막 에코도 끝나면
            if (point_index == -1)
            {
                //딜레이 후 리스너 디엑티브
                if (deactive_delay.AutoTimer())
                {
                    start_listener.DeActive();
                    end_listener.DeActive();
                }
            }
        }
    }
}
