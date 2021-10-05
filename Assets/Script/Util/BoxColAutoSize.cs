using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class BoxColAutoSize : MonoBehaviour {

    private SpriteRenderer render;
    private BoxCollider2D box_col;
    public float height = 0.1f;

    //오브젝트 변경 시 불러짐
    private void OnRenderObject()
    {
        render = GetComponent<SpriteRenderer>();
        box_col = GetComponent<BoxCollider2D>();
        ResizeCollider();
    }

    //컬리더 크기 자동으로 바꾸기
    private void ResizeCollider()
    {
        float render_width = render.size.x;
        float render_height = render.size.y;
        float box_height = render_height * height;
        
        box_col.size = new Vector2(render_width, box_height);
        box_col.offset = new Vector2(0, (render_height - box_height) / 2.0f);
    }
}
