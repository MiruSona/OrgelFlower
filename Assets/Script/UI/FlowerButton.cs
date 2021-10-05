using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Define;

public class FlowerButton : ButtonBase {

    //참조
    private PlayerData player_data;
    private PlayerLamp player_lamp;

    //컴포넌트
    private Image flower_image;
    private Image outline;

    //스프라이트
    private Sprite no_flower;

    //버튼 지정
    public FlowerColor color = FlowerColor.None;

    //On/Off 색
    private Color flower_off = new Color(0.3f, 0.3f, 0.3f);
    private Color flower_on = new Color(1.0f, 1.0f, 0.3f);
    private Color flower_continue = new Color(1.0f, 0.8f, 0.3f);

    //버튼 작동 여부
    private bool interactive = true;    

    //지속 관련
    private bool continues = false;                             //지속 여부
    private Timer check_continue_timer = new Timer(0, 1.0f);    //지속 할지 결정할 타이머

    private void Start()
    {
        //참조
        player_data = DataManager.player_data;
        player_lamp = FindObjectOfType<PlayerLamp>();

        //컴포넌트
        flower_image = transform.GetChild(0).GetComponent<Image>();
        outline = GetComponent<Image>();

        //이미지
        no_flower = Resources.Load<Sprite>("Sprite/Flower/FlowerLeaf/NoFlower");

        //색처리
        outline.color = flower_off;
    }

    private void Update()
    {
        //버튼 상태 변경
        ButtonUpdate();
    }

    //버튼 상태 변경
    private void ButtonUpdate()
    {
        //꽃 받기
        FlowerData flower = player_data.GetFlowerByColor(color);

        //꽃이 있으면
        if (flower != null)
        {
            //수명 있으면 버튼 킴
            if (flower.CheckLife())
            {
                //버튼 켜기
                if (!interactive)
                    interactive = true;

                //이미지 변경
                flower_image.sprite = flower.GetLeafSpriteByLife();

                //꽃 있나 확인
                if (player_data.flower_current != null)
                {
                    //선택한 꽃과 다르면 아웃라인 끄기
                    if (color != player_data.flower_current.flower_color)
                        outline.color = flower_off;
                    //같으면 버튼 유지할지 체크
                    else
                    {
                        if (!check_continue_timer.CheckTimer())
                            check_continue_timer.AddTimer();
                        else
                        {
                            outline.color = flower_continue;
                            continues = true;
                        }
                    }
                }
                //선택한 꽃이 없어도 끄기
                else
                    outline.color = flower_off;
            }
            //수명 없으면 버튼 끔
            else
            {
                if (interactive)
                {
                    interactive = false;
                    flower_image.sprite = no_flower;
                    outline.color = flower_off;
                    check_continue_timer.InitTimer();
                }
            }
        }
    }

    //꽃 선택
    public void SelectFlowerBtn()
    {
        //꽃 받기
        FlowerData flower = player_data.flower_current;

        //꽃이 없다면 바로 꽃 선택
        if (flower == null)
        {
            player_data.SelectFlower(color);
            outline.color = flower_on;

            //램프 에코 타이머 초기화
            player_lamp.InitEchoTimer();
        }
        //선택한 꽃이 있다면
        else
        {
            //이미 선택한 꽃이면 꺼줌
            if (color == flower.flower_color)
                OffFlowerBtn();
            //아니라면 선택한 꽃을 넣어줌
            else
            {
                player_data.SelectFlower(color);
                outline.color = flower_on;

                //램프 에코 타이머 초기화
                player_lamp.InitEchoTimer();
            }
        }
    }

    //꽃 선택 해제
    public void OffFlowerBtn()
    {
        player_data.SelectFlower(FlowerColor.None);
        outline.color = flower_off;
        check_continue_timer.InitTimer();
        continues = false;
    }

    //들어올 시
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        //버튼 작동 중이면 꽃 선택
        if (interactive)
            SelectFlowerBtn();
    }

    //나갈 시
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        //버튼 작동 중 +  유지 안하면 선택 해제
        if (interactive && !continues)
            OffFlowerBtn();
    }
}
