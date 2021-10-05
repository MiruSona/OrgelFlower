using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MovingButton : ButtonBase
{
    //참조
    private MainPlayer player;
    private PlayerData player_data;

    //컴포넌트
    private Image image;

    //설정
    public enum MoveKind
    {
        Left, Right
    }
    public MoveKind kind = MoveKind.Right;

    //색
    public float ColorChnageSpeed = 0.1f;
    public Color NormalColor = Color.white;
    public Color PressedColor = Color.gray;

    //초기화
    void Start () {
        //참조
        player = MainPlayer.instance;
        player_data = DataManager.player_data;

        //컴포넌트
        image = GetComponent<Image>();
    }

    //색변경
    private void FixedUpdate()
    {
        if (player_data.state != PlayerData.State.Alive)
            return;

        if (pressed)
        {
            ChangeColor(PressedColor);
            Moving();
        }
        else
            ChangeColor(NormalColor);
    }

    //움직임 처리
    private void Moving()
    {
        switch (kind)
        {
            case MoveKind.Left: player.Move(-1.0f); break;
            case MoveKind.Right: player.Move(1.0f); break;
        }
    }

    //색변경
    private void ChangeColor(Color _target)
    {
        image.color = Color.Lerp(image.color, _target, ColorChnageSpeed);
    }
}
