using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class BlockChameleon : MonoBehaviour {

    //참조
    private PlayerData player_data;

    //컴포넌트
    private FlowerListenerMulti listener;
    private SpriteRenderer inside, border, symbol;
    private Collider2D inside_col;

    //기능
    public enum Operation
    {
        Show,
        Hide,
        Both
    }
    public Operation operation = Operation.Show;

    //체크할 색
    private FlowerColor ShowColor = FlowerColor.Red;
    private FlowerColor HideColor = FlowerColor.Blue;

    //상징 색
    private Color symbol_show_color = new Color(1.0f, 0.5f, 0.5f);
    private Color symbol_hide_color = new Color(0.5f, 0.5f, 1.0f);

    //옵션
    public bool show = true;

    //색관련
    private float delta_color = 0.05f;
    private const float min_color = 0f;
    private const float max_color = 1f;

    //플레이어 공격 관련
    private bool player_overlap = false;

	//초기화
	void Start () {

        //참조
        player_data = DataManager.player_data;

        //컴포넌트 초기화
        border = GetComponent<SpriteRenderer>();
        inside = transform.GetChild(0).GetComponent<SpriteRenderer>();
        symbol = transform.GetChild(1).GetComponent<SpriteRenderer>();
        inside_col = transform.GetChild(0).GetComponent<Collider2D>();

        //리스너 초기화
        listener = GetComponent<FlowerListenerMulti>();

        //색 초기화 / 리스너 초기화
        Color color = Color.white;
        switch (operation)
        {
            //숨기기
            case Operation.Hide:
                listener.CheckColors = new FlowerColor[1] { HideColor };
                //외각선 색칠
                SetSpriteToFlowerColor(border, HideColor, 1.0f);
                break;
            //보이기
            case Operation.Show:
                listener.CheckColors = new FlowerColor[1] { ShowColor };
                //외각선 색칠
                SetSpriteToFlowerColor(border, ShowColor, 1.0f);
                //투명하게 미리 만듬
                color = inside.color;
                color.a = 0f;
                inside.color = color;
                inside_col.enabled = false;
                break;
            //둘다
            case Operation.Both:
                listener.CheckColors = new FlowerColor[2] { HideColor, ShowColor };
                //시작 시 꺼져있으면 투명하게 미리 만듬
                if (!show)
                {
                    color = inside.color;
                    color.a = 0f;
                    inside.color = color;
                    color = symbol.color;
                    color.a = 0f;
                    symbol.color = color;
                    inside_col.enabled = false;
                }
                //리스너 항상 작동
                listener.always_active = true;
                break;
        }
    }
	
	//업데이트
	void FixedUpdate () {

        //리스너 체크
        if (listener.GetActive())
        {
            //기능에 따라 실행
            switch (operation)
            {
                //숨기기
                case Operation.Hide: HideActive(); break;
                //보이기
                case Operation.Show: ShowActive(); break;
                //둘다
                case Operation.Both:
                    if(listener.GetCurrentColor() == HideColor)
                        HideActive();
                    else if (listener.GetCurrentColor() == ShowColor)
                        ShowActive();
                    break;
            }
        }
        //리스너 꺼졌으면 되돌아오게
        else
        {
            //기능에 따라 실행
            switch (operation)
            {
                //숨기기
                case Operation.Hide: ShowActive(); break;
                //보이기
                case Operation.Show: HideActive(); break;
            }
        }
	}

    //숨기기
    private void HideActive()
    {
        //색 점점 투명하게
        Color color = inside.color;
        if (color.a > min_color)
            color.a -= delta_color;
        else
        {
            color.a = min_color;
            inside_col.enabled = false;
        }
        inside.color = color;

        //기능에 따라 상징 색 다르게
        switch (operation)
        {
            //보이기
            case Operation.Show:
                SetSpriteToFlowerColor(symbol, FlowerColor.None, color.a);
                break;
            //숨기기 / 둘다 - 숨기는 색으로
            case Operation.Hide:
            case Operation.Both:
                SetSpriteToColor(symbol, symbol_hide_color, color.a);
                break;
        }
    }

    //보이기
    private void ShowActive()
    {
        //색 점점 뚜렷해지게
        Color color = inside.color;
        if (color.a < max_color)
            color.a += delta_color;
        else
        {
            color.a = max_color;

            //플레이어가 안겹쳐있으면 컬리더 켜짐
            if (!player_overlap)
                inside_col.enabled = true;
            //겹쳐있으면 플레이어 공격
            else
                player_data.SubHP();
        }
        inside.color = color;

        //기능에 따라 상징 색 다르게
        switch (operation)
        {
            //숨기기
            case Operation.Hide:
                SetSpriteToFlowerColor(symbol, FlowerColor.None, color.a);
                break;
            //보이기 / 둘다 - 보이는 색으로
            case Operation.Show:
            case Operation.Both:
                SetSpriteToColor(symbol, symbol_show_color, color.a);
                break;
        }
    }

    //색 처리
    private void SetSpriteToFlowerColor(SpriteRenderer _render, FlowerColor _color, float _alpha)
    {
        Color color = ColorUtil.GetFlowerColor(_color);
        color.a = _alpha;
        _render.color = color;
    }

    private void SetSpriteToColor(SpriteRenderer _render, Color _color, float _alpha)
    {
        Color color = _color;
        color.a = _alpha;
        _render.color = color;
    }
    
    #region 플레이어 겹쳐있나 체크
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerOverlap overlap = collision.GetComponent<PlayerOverlap>();
        if (overlap != null)
            player_overlap = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerOverlap overlap = collision.GetComponent<PlayerOverlap>();
        if (overlap != null)
            player_overlap = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            player_overlap = false;
    }
    #endregion
}
