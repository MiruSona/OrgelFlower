using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[RequireComponent(typeof(FlowerListenerMulti))]
public class BlockJump : MonoBehaviour {

    //참조
    private PlayerData player_data;
    private MainPlayer player;

    //컴포넌트
    private FlowerListenerMulti listener;
    private SpriteRenderer symbol;
    
    //체크할 색
    public FlowerColor HighJumpColor = FlowerColor.Red;
    public FlowerColor LowJumpColor = FlowerColor.Blue;

    //점프력 관련
    private float jump_power = 0f;
    public float high_jump_power = 4f;
    public float low_jump_power = 2f;

    //블럭에 닿았나 체크
    private bool on_block = false;

    //초기화
    void Start () {
        //참조
        player_data = DataManager.player_data;
        player = MainPlayer.instance;

        //컴포넌트 초기화
        symbol = transform.GetChild(0).GetComponent<SpriteRenderer>();

        //리스너 초기화
        listener = GetComponent<FlowerListenerMulti>();
        listener.CheckColors = new FlowerColor[2] { HighJumpColor, LowJumpColor };
        listener.always_active = true;
    }
	
	//업데이트
	void FixedUpdate () {
        //점프 파워 설정
        SetJumpPower();

        //블럭에 닿았다면 점프!(리스너 켜진지도 체크)
        if (on_block && player_data.ground && listener.GetActive())
            player.Jump();
    }

    //점프 파워 설정
    private void SetJumpPower()
    {
        //낮은거 엑티브 됬으면
        if (listener.GetCurrentColor() == LowJumpColor)
        {
            //낮은점프력 세팅
            jump_power = low_jump_power;
            //화살표 색 바꿈
            symbol.color = ColorUtil.GetFlowerColor(LowJumpColor);
        }
        //높은거 엑티브 됬으면
        else if (listener.GetCurrentColor() == HighJumpColor)
        {
            //높은 쩜프력 세팅
            jump_power = high_jump_power;
            //화살표 색 바꿈
            symbol.color = ColorUtil.GetFlowerColor(HighJumpColor);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어면 점프++
        if (collision.CompareTag("Player"))
        {
            player_data.jump_power_add = jump_power;
            on_block = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //플레이어면 점프++
        if (collision.CompareTag("Player"))
        {
            player_data.jump_power_add = jump_power;
            on_block = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어면 다시 되돌림
        if (collision.CompareTag("Player"))
        {
            player_data.jump_power_add = 0f;
            on_block = false;
        }
    }
}
