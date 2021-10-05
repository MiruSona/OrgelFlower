using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class ScalingBlock : MonoBehaviour {

    //컴포넌트
    private SpriteRenderer[] renders;

    //조절값
    private float sprite_width = 1.0f, sprite_height = 1.0f;
    public float WidthNum = 1.0f, HeightNum = 1.0f;

#if UNITY_EDITOR

    //초기화
    public void Init () {
        renders = GetComponentsInChildren<SpriteRenderer>();
        if(renders != null)
        {
            Sprite sprite = renders[0].sprite;
            if (sprite != null)
            {
                //가로 / 세로 초기값 구하기
                sprite_width = sprite.rect.width / sprite.pixelsPerUnit;
                sprite_height = sprite.rect.height / sprite.pixelsPerUnit;
            }
        }
    }
	
	//블럭 크기 설정
    public void UpdateScale()
    {
        foreach(SpriteRenderer render in renders)
            render.size = new Vector2(sprite_width * WidthNum, sprite_height * HeightNum);
    }

#endif
}
