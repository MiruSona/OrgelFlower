using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class BlockBase : MonoBehaviour {

    //참조
    private FlowerListener listener;

    //컴포넌트
    private SpriteRenderer core_render;

    //색
    public Color ColorOff = Color.white;

    //초기화
    void Start () {
        listener = GetComponent<FlowerListener>();
        core_render = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
	
	//색 변경
	void Update () {
        if (listener.GetActive())
            core_render.color = ColorUtil.GetFlowerColor(listener.CheckColor);
        else
            core_render.color = ColorOff;
    }
}
