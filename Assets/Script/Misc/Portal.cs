using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    //참조
    private PlayerData player_data;
    private FadeUI fade_ui;
    private InteractButton interact_btn;

    //컴포넌트
    private SpriteRenderer sprite_render;

    //원하는 씬
    public string SceneName = "";

    //플레이어 닿았나 체크
    private bool check_player = false;

    //포탈 열렸나 표시
    public bool Open = false;

    //이미지
    private const string path = "Sprite/Misc/Door/";
    private Sprite door_open, door_close;

    //초기화
    private void Start()
    {
        player_data = DataManager.player_data;
        fade_ui = FadeUI.instance;
        interact_btn = FindObjectOfType<InteractButton>();

        sprite_render = GetComponent<SpriteRenderer>();
        door_open = Resources.Load<Sprite>(path + "DoorOpen");
        door_close = Resources.Load<Sprite>(path + "DoorClose");
    }

    //업데이트
    private void Update()
    {
        //문 열림 여부에 따라 스프라이트 변경
        if (Open)
            sprite_render.sprite = door_open;
        else
            sprite_render.sprite = door_close;

        //플레이어 닿음 여부 && 문 열림 여부 && 버튼 누름 여부
        if (check_player && Open && interact_btn.interactive)
        {
            //페이드 아웃
            fade_ui.FadeOut();

            //끝났으면 씬 넘어가기
            if (fade_ui.GetDone())
            {
                //빈칸 아니면 씬 로드
                if (SceneName.Trim() != "")
                {
                    SceneManager.LoadScene(SceneName);
                    player_data.Init();
                }
            }
        }
    }

    #region 충돌 체크
    private void OnTriggerStay2D(Collider2D collision)
    {
        //플레이어 들어왔다 표시
        if (collision.CompareTag("Player") && Open)
        {
            check_player = true;
            interact_btn.ChangeButton(InteractButton.Kind.Door);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어 나갔다 표시
        if (collision.CompareTag("Player"))
        {
            check_player = false;
            interact_btn.ChangeButton(InteractButton.Kind.Jump);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //플레이어 들어왔다 표시
        if (collision.collider.CompareTag("Player") && Open)
        {
            check_player = true;
            interact_btn.ChangeButton(InteractButton.Kind.Door);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //플레이어 나갔다 표시
        if (collision.collider.CompareTag("Player"))
        {
            check_player = false;
            interact_btn.ChangeButton(InteractButton.Kind.Jump);
        }
    } 
    #endregion

    //플레이어 닿음 여부 반환
    public bool GetCheckPlayer()
    {
        return check_player && interact_btn.interactive;
    }

}
