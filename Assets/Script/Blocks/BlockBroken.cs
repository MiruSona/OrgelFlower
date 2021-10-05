using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class BlockBroken : MonoBehaviour {
    
    //컴포넌트
    private SpriteRenderer render;
    private Animator anim;
    private Collider2D col;

    //투명해지게
    private bool broken = false;
    [SerializeField]
    private float delta_color = 0.01f;
    private const float min_color = 0f;
    private const float max_color = 1f;

    //타이머
    [SerializeField]
    private float RestoreTime = 5f;
    private Timer restore_timer = new Timer(0, 5f);

	//초기화
	void Start () {
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        restore_timer.limit = RestoreTime;
    }
	
	//업데이트
	void FixedUpdate () {
        //부서진거 체크
        if (broken)
        {
            //애니메이션 실행
            anim.SetTrigger("Broken");

            //투명해지기
            Color color = render.color;
            if (color.a > min_color)
                color.a -= delta_color;
            else
            {
                color.a = min_color;
                col.enabled = false;
            }
            render.color = color;

            //복구시간 다됬는지 체크
            if (restore_timer.AutoTimer())
                broken = false;
        }
        //안부셔 졌다면 색 되돌리기 + 컬리더 복구
        else
        {
            //애니메이션 실행
            anim.SetTrigger("Normal");

            //보이기
            Color color = render.color;
            if (color.a < max_color)
                color.a += delta_color;
            else
            {
                color.a = max_color;
                col.enabled = true;
            }
            render.color = color;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어 땅 체크랑 부딪치면
        GroundCheck ground_check = collision.GetComponent<GroundCheck>();
        if (ground_check != null)
        {
            //부서짐 표시 + 컬리더 끄기
            broken = true;
        }
    }
}
