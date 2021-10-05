using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CopyBlocks : MonoBehaviour {

    //복사할 오브젝트
    public Transform Target = null;

    //리스트
    private Stack<GameObject> target_stack = new Stack<GameObject>();

    //조절값
    private float sprite_width = 1.0f, sprite_height = 1.0f;
    public float offset = 0f;
    public int WidthNum = 0, HeightNum = 0;

#if UNITY_EDITOR

    //초기화
    public void Init()
    {
        //복사할꺼 있나 체크
        if (Target != null)
        {
            SpriteRenderer render = Target.GetComponent<SpriteRenderer>();
            if(render != null)
            {
                Sprite sprite = render.sprite;
                if (sprite != null)
                {
                    //가로 / 세로 초기값 구하기
                    sprite_width = sprite.rect.width / sprite.pixelsPerUnit;
                    sprite_height = sprite.rect.height / sprite.pixelsPerUnit;
                }
            }

            //차일드 있으면 리스트에 미리 넣어둠
            foreach(Transform t in transform)
                target_stack.Push(t.gameObject);
        }
    }

    //블럭 생성
    public void CreateBlock()
    {
        //복사할꺼 있나 체크
        if (Target != null)
        {
            //가로 수 만큼 생성
            for (int i = 0; i < WidthNum; i++)
            {
                Transform temp = PrefabUtility.InstantiatePrefab(Target) as Transform;
                temp.SetParent(transform);
                temp.name = Target.name + i;
                Vector3 pos = transform.position;
                pos.x += (sprite_width + offset) * i;
                temp.position = pos;

                target_stack.Push(temp.gameObject);
            }

            //세로 수 만큼 생성
            for (int i = 0; i < HeightNum; i++)
            {
                Transform temp = PrefabUtility.InstantiatePrefab(Target) as Transform;
                temp.SetParent(transform);
                temp.name = Target.name + i;
                Vector3 pos = transform.position;
                if(WidthNum > 0)
                    pos.y += (sprite_height + offset) * (i + 1);
                else
                    pos.y += (sprite_height + offset) * i;
                temp.position = pos;

                target_stack.Push(temp.gameObject);
            }
        }
    }

    //블럭 초기화
    public void ResetBlock()
    {
        int num = target_stack.Count;
        for (int i = 0; i < num; i++)
            DestroyImmediate(target_stack.Pop());
    }

#endif
}
