using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScalingBlock))]
public class ScalingBlockEditor : Editor {

    //컴포넌트
    private ScalingBlock scaling_block;

    //초기화
    private void OnEnable()
    {
        scaling_block = (ScalingBlock)target;
        if(scaling_block != null)
            scaling_block.Init();
    }

    //인스펙터창 변형
    public override void OnInspectorGUI()
    {
        //블럭 가로 갯수
        EditorGUI.BeginChangeCheck();
        scaling_block.WidthNum = CreateSlider("Width Num", scaling_block.WidthNum, 0.1f, 100f);
        if (EditorGUI.EndChangeCheck())
        {
            scaling_block.UpdateScale();
            Undo.RecordObject(target, "Block Width Num Change");
        }

        //블럭 세로 갯수
        EditorGUI.BeginChangeCheck();
        scaling_block.HeightNum = CreateSlider("Height Num", scaling_block.HeightNum, 0.1f, 100f);
        if (EditorGUI.EndChangeCheck())
        {
            scaling_block.UpdateScale();
            Undo.RecordObject(target, "Block Height Num Change");
        }
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
