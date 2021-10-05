using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CopyBlocks))]
public class CopyBlockEditor : Editor
{
    //컴포넌트
    private CopyBlocks copy_blocks;

    //초기화
    private void OnEnable()
    {
        copy_blocks = (CopyBlocks)target;
        if (copy_blocks != null)
            copy_blocks.Init();
    }

    //인스펙터창 변형
    public override void OnInspectorGUI()
    {
        //복사할 타겟
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        copy_blocks.Target = (Transform)EditorGUILayout.ObjectField("Target", copy_blocks.Target, typeof(Transform), true);
        GUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Set Target");

        //블럭 가로 갯수
        EditorGUI.BeginChangeCheck();
        copy_blocks.WidthNum = (int)CreateSlider("Width Num", copy_blocks.WidthNum, 0, 100f);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Copy Target Width");

        //블럭 세로 갯수
        EditorGUI.BeginChangeCheck();
        copy_blocks.HeightNum = (int)CreateSlider("Height Num", copy_blocks.HeightNum, 0, 100f);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Copy Target Height");

        //오프셋
        EditorGUI.BeginChangeCheck();
        copy_blocks.offset = CreateSlider("Offset", copy_blocks.offset, -10f, 10f);
        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Offset Change");


        GUILayout.BeginHorizontal();
        //생성 버튼
        if (GUILayout.Button("Create Blocks"))
        {
            copy_blocks.ResetBlock();
            copy_blocks.CreateBlock();
        }
        //제거 버튼
        if (GUILayout.Button("Destroy Blocks"))
        {
            copy_blocks.ResetBlock();
        }
        GUILayout.EndHorizontal();
    }
    
    //슬라이더 생성
    private float CreateSlider(string _label_name, float _slider_pos, float _min, float _max)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(_label_name);
        _slider_pos = EditorGUILayout.Slider(_slider_pos, _min, _max, null);
        GUILayout.EndHorizontal();

        return _slider_pos;
    }
}
