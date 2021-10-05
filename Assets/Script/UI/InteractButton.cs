using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InteractButton : ButtonBase
{
    //참조
    private MainPlayer player;
    private PlayerData player_data;

    //컴포넌트
    private Image image;

    //누름 여부
    [HideInInspector]
    public bool interactive = false;

    //설정
    public enum Kind
    {
        Jump, Door
    }
    private Kind kind = Kind.Jump;

    //색
    public float ColorChnageSpeed = 0.1f;
    public Color JumpColor = new Color32(255, 169, 88, 255);
    public Color DoorColor = new Color32(147, 205, 255, 255);
    public Color PressedColor = Color.gray;

    //초기화
    void Start()
    {
        //참조
        player = MainPlayer.instance;
        player_data = DataManager.player_data;

        //컴포넌트
        image = GetComponent<Image>();
    }

    //색변경
    private void Update()
    {
        if (player_data.state != PlayerData.State.Alive)
            return;

        if (interactive)
        {
            ChangeColor(PressedColor);
            Interactive();
        }
        else
        {
            switch (kind)
            {
                case Kind.Jump: ChangeColor(JumpColor); break;
                case Kind.Door: ChangeColor(DoorColor); break;
            }
        }
    }

    //상황에 따른 처리
    private void Interactive()
    {
        switch (kind)
        {
            case Kind.Jump: player.Jump(); break;
            case Kind.Door: break;
        }
    }

    //버튼 변경
    public void ChangeButton(Kind _kind)
    {
        kind = _kind;
    }

    //색변경
    private void ChangeColor(Color _target)
    {
        image.color = Color.Lerp(image.color, _target, ColorChnageSpeed);
    }

    //드래그 / 엔터 일때
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        interactive = true;
    }

    //나가면
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        //문일경우 false 안함(넘어가는거 처리 때문에..)
        if (kind != Kind.Door)
            interactive = false;
    }
}
