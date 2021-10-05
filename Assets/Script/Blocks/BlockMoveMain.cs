using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[RequireComponent(typeof(FlowerListener))]
public class BlockMoveMain : MonoBehaviour {

    //참조
    private Transform player;

    //컴포넌트
    private FlowerListener listener;
    private SpriteRenderer symbol;
    
	//초기화
	void Start () {
        player = MainPlayer.instance.transform;
        listener = GetComponent<FlowerListener>();
        symbol = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    //플레이어가 블럭위에 있고 작동중이면 X축 고정!
    void FixedUpdate()
    {
        //리스너 켜졌으면 보석 칠하기
        if (listener.GetActive())
            symbol.color = ColorUtil.GetFlowerColor(listener.CheckColor);
        //꺼졌다면 꺼졌다 보석 표시
        else
            symbol.color = ColorUtil.GetFlowerColor(FlowerColor.None);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 닿으면 플레이어를 이 오브젝트 하위로 둠
        if (collision.CompareTag("Player"))
            player.SetParent(transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //플레이어가 닿으면 플레이어를 이 오브젝트 하위로 둠
        if (collision.CompareTag("Player") && player.parent == null)
            player.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //벗어나면 플레이어 페런트 없도록
        if (collision.CompareTag("Player"))
            player.SetParent(null);
    }
}
