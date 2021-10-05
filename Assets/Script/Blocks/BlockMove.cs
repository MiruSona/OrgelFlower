using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class BlockMove : MonoBehaviour {

    //참조
    private FlowerListener listener;
    private SpriteRenderer[] points_render;

    //컴포넌트
    private Transform main;
    private Transform[] points;
    private Rigidbody2D main_rb2d;
    private LineRenderer line_render;
    
    //이동 관련
    public float move_speed = 0.01f;    //움직이는 속도
    private bool move_forward = true;   //앞으로 움직이는지 체크
    private int point_index = 0;        //포인트 인덱스

    //대기 시간
    public float DelayTime = 1f;
    private Timer delay_timer;

    //되돌아가기 여부
    public bool GoBack = true;

    //초기화
    void Start () {
        //참조
        listener = GetComponentInChildren<FlowerListener>();
        main = listener.transform;

        //리스너 계속 켜지게 설정
        listener.always_active = true;

        //포인트
        points_render = GetComponentInChildren<CopyBlocks>().GetComponentsInChildren<SpriteRenderer>();
        points = new Transform[points_render.Length];
        for (int i = 0; i < points.Length; i++)
            points[i] = points_render[i].transform;

        //컴포넌트
        line_render = GetComponent<LineRenderer>();
        line_render.sortingLayerName = "Ground";
        line_render.sortingOrder = -1;
        line_render.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
            line_render.SetPosition(i, points[i].position);

        //대기시간
        delay_timer = new Timer(0, DelayTime);
    }
	
	//업데이트
	void FixedUpdate () {
        //색변경
        ChangePointsColor(listener.GetActive());

        //활성화 됬으면 움직임
        if (listener.GetActive())
            MoveToPoint();
    }

    //포인트 색 변경
    private void ChangePointsColor(bool _active)
    {
        //활성화 되면
        if (_active)
        {
            //색변경
            foreach (SpriteRenderer render in points_render)
                render.color = ColorUtil.GetFlowerColor(listener.CheckColor);
            line_render.startColor = ColorUtil.GetFlowerColor(listener.CheckColor);
            line_render.endColor = ColorUtil.GetFlowerColor(listener.CheckColor);
        }
        //비활성화 되면
        else
        {
            foreach (SpriteRenderer render in points_render)
                render.color = ColorUtil.GetFlowerColor(FlowerColor.None);
            line_render.startColor = ColorUtil.GetFlowerColor(FlowerColor.None);
            line_render.endColor = ColorUtil.GetFlowerColor(FlowerColor.None);
        }
    }

    //이동
    private void MoveToPoint()
    {
        Vector2 target_pos = Vector2.zero;

        //현재 인덱스의 위치 받음
        target_pos = points[point_index].position;

        //거리가 움직이는 속도 보다 클때 움직임
        if (Vector2.Distance(main.position, target_pos) > move_speed)
            main.position = Vector2.MoveTowards(main.position, target_pos, move_speed);
        else
        {
            //앞으로 움직이는거면
            if (move_forward)
            {
                //인덱스가 마지막까지 이동
                if (point_index < points.Length - 1)
                    point_index++;
                //끝에 도달했으면 
                else
                {
                    //끝으로 이동
                    main.position = target_pos;

                    //딜레이
                    if (delay_timer.AutoTimer())
                    {
                        //인덱스-- / 뒤로 움직임 / 리스너 끄기
                        point_index--;
                        move_forward = false;

                        //되돌아가지 않는 설정일 경우 여기서 멈춤
                        if(!GoBack)
                            listener.DeActive();
                    }
                }
            }
            //뒤로 움직이는 거면
            else
            {
                //인덱스 처음까지 이동
                if (point_index > 0)
                    point_index--;
                //끝에 도달했으면
                else
                {
                    //끝으로 이동
                    main.position = target_pos;

                    //딜레이
                    if (delay_timer.AutoTimer())
                    {
                        //인덱스++ / 앞으로 움직임 / 리스너 끄기                        
                        point_index++;
                        move_forward = true;
                        listener.DeActive();
                    }
                }
            }
        }
    }
}
