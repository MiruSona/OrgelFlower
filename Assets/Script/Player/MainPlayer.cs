using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class MainPlayer : SingleTon<MainPlayer> {
    
    //참조
    private PlayerData player_data;
    private MovingButton[] moving_buttons;

    //컴포넌트
    private Rigidbody2D rb2d;
    private Animator anim;
    
    //조절값
    private float scale = 0.5f;
    private const float move_offset = 3.0f;
    private Timer jump_anim_delay = new Timer(0, 0.1f);
    
    //테스트용
    public bool PCMode = false;

    //초기화
    void Start()
    {
        //참조
        player_data = DataManager.player_data;
        moving_buttons = FindObjectsOfType<MovingButton>();

        //컴포넌트
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //초기화
        scale = transform.localScale.x;
    }

    //업데이트
    void FixedUpdate()
    {
        //상태처리
        switch (player_data.state)
        {
            case PlayerData.State.Alive:
                Alive();

                //입력 처리
                if (PCMode)
                    InputKey();
                else
                    CheckNoTouch();

                break;
            case PlayerData.State.Attacked:
                Attacked();
                return;
            case PlayerData.State.Die:
                Die();
                return;
        }
    }
    
    #region 입력처리
    //키보드 입력
    private void InputKey()
    {
        //좌,우 움직임
        if (Input.GetKey(KeyCode.A))
        {
            player_data.facing = player_data.LEFT;
            Move();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            player_data.facing = player_data.RIGHT;
            Move();
        }
        //둘다 안누르면 좌우이동 멈추기
        else
            MoveStop();

        //점프
        if (Input.GetKey(KeyCode.Space))
            Jump();
    }

    //터치 입력(움직이는 버튼 안누르는지 체크)
    private void CheckNoTouch()
    {
        //일단 true
        bool no_touch = true;
        //버튼 하나라도 누른거 있으면 false
        for (int i = 0; i < moving_buttons.Length; i++)
        {
            if (moving_buttons[i].pressed)
                no_touch = false;
        }

        //아무것도 터치 안했으면 움직임 멈춤
        if (no_touch)
            MoveStop();
    }
    #endregion

    #region 움직임
    //멈춤
    public void MoveStop()
    {
        //현재속도 받기
        Vector2 velocity = rb2d.velocity;
        //0값 대입
        velocity.x = 0f;
        rb2d.velocity = velocity;
    }

    //플랫포머 움직임
    private void Move()
    {
        //현재속도 받기
        Vector2 velocity = rb2d.velocity;
        //속도 * 방향값 대입
        velocity.x = player_data.speed * player_data.facing;
        rb2d.velocity = velocity;

        //좌우 변경
        transform.localScale = new Vector3(-player_data.facing, 1, 1) * scale;
    }

    public void Move(float _offset)
    {
        //오프셋에 따라 플레이어 좌우 변경
        if(_offset < 0)
            player_data.facing = player_data.LEFT;
        else
            player_data.facing = player_data.RIGHT;
        
        //현재속도 받기
        Vector2 velocity = rb2d.velocity;
        //속도 * 방향값 대입
        velocity.x = player_data.speed * _offset;
        rb2d.velocity = velocity;

        //좌우 변경
        transform.localScale = new Vector3(-player_data.facing, 1, 1) * scale;
    }

    //플랫포머 점프
    public void Jump()
    {
        //땅인지 체크
        if (player_data.ground)
        {
            //현재속도 받기
            Vector2 velocity = rb2d.velocity;
            //점프!
            velocity.y = player_data.jump_power + player_data.jump_power_add;
            rb2d.velocity = velocity;
        }
    }

    #endregion

    #region 상태 처리
    private void Alive()
    {
        //애니메이션 처리
        if (!player_data.ground)
        {
            //점프애니메이션은 딜레이 약간 줌
            if (!jump_anim_delay.CheckTimer())
                jump_anim_delay.AddTimer();
            else
                anim.SetTrigger("Jump");
            //땅이 아니면 움직인거로 표시
            player_data.move = true;
        }
        else
        {
            //애니메이션 딜레이 초기화
            jump_anim_delay.InitTimer();

            //가만히 있으면 안움직인 거로 표시
            if (rb2d.velocity.x == 0f)
            {
                anim.SetTrigger("Idle");
                player_data.move = false;
            }
            else
            {
                anim.SetTrigger("Walk");
                player_data.move = true;
            }
        }
    }

    //피격 - 스테이지 재시작
    private void Attacked()
    {
        //멈추기
        MoveStop();

        //피격 모습
        anim.SetTrigger("Hit");
    }

    //죽음 - 길 재시작
    private void Die()
    {
        //멈추기
        MoveStop();

        //피격 모습
        anim.SetTrigger("Hit");
    }
    #endregion
}
