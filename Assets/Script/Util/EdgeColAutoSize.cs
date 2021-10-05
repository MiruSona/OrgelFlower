using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class EdgeColAutoSize : MonoBehaviour {

    private SpriteRenderer render;
    private EdgeCollider2D edge_col;

    //오브젝트 변경 시 불러짐
    private void OnRenderObject()
    {
        render = GetComponent<SpriteRenderer>();
        edge_col = GetComponent<EdgeCollider2D>();
        ResizeCollider();
    }

    //컬리더 크기 자동으로 바꾸기
    private void ResizeCollider()
    {
        float width_half = render.size.x / 2.0f;
        float height_half = render.size.y / 2.0f;

        Vector2[] points = new Vector2[edge_col.pointCount];
        points[0] = new Vector2(-width_half, height_half);
        points[1] = new Vector2(width_half, height_half);

        edge_col.points = points;
    }
}
