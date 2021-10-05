using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class PlayerData {

    //스탯
    public readonly float hp_max = 3f;
    public float hp = 3f;

    //물리
    public readonly float speed = 3.5f;
    public readonly float jump_power = 8.0f;
    public float jump_power_add = 0f;
    public readonly float LEFT = -1f, RIGHT = 1f, UP = 1f, DOWN = -1f;
    public float facing = 1f;
    public Vector2 direction = Vector2.zero;

    //상태
    public enum State
    {
        Alive,
        Attacked,
        Die
    }
    public State state = State.Alive;

    //움직임
    public bool move = false;
    public bool ground = false;

    //꽃
    public FlowerData flower_y, flower_b, flower_r;
    public FlowerData flower_current = null;
    
    #region HP 관련
    public void AddHP()
    {
        if (state == State.Die)
            return;

        if (hp + 1.0f < hp_max)
            hp++;
        else
            hp = hp_max;
    }

    public void SubHP()
    {
        if (state != State.Alive)
            return;

        if (hp - 1.0f >= 0)
        {
            hp--;
            state = State.Attacked;
        }
        else
        {
            hp = 0;
            state = State.Die;
        }
    }

    public void InitHP()
    {
        hp = hp_max;
    }
    #endregion
    
    #region 꽃 관련
    //수명 추가
    public void SetFlowerLife(FlowerColor _color, int _life)
    {
        switch (_color)
        {
            case FlowerColor.Yellow: flower_y.SetLife(_life); break;
            case FlowerColor.Blue: flower_b.SetLife(_life); break;
            case FlowerColor.Red: flower_r.SetLife(_life); break;
        }
    }

    //꽃 선택 - UI용
    public void SelectFlower(FlowerColor _color)
    {
        switch (_color)
        {
            case FlowerColor.None: flower_current = null; break;        //비선택
            case FlowerColor.Yellow: flower_current = flower_y; break;  //노란 꽃
            case FlowerColor.Blue: flower_current = flower_b; break;    //파란 꽃
            case FlowerColor.Red: flower_current = flower_r; break;     //붉은 꽃
        }

        //꽃 수명 체크
        if(flower_current != null)
        {
            if (!flower_current.CheckLife())
                flower_current = null;
        }
    }

    //꽃 색에 따라 받기
    public FlowerData GetFlowerByColor(FlowerColor _color)
    {
        FlowerData data = null;
        switch (_color)
        {
            case FlowerColor.None: data = null; break;        //비선택
            case FlowerColor.Yellow: data = flower_y; break;  //노란 꽃
            case FlowerColor.Blue: data = flower_b; break;    //파란 꽃
            case FlowerColor.Red: data = flower_r; break;     //붉은 꽃
        }

        return data;
    }
    #endregion

    //생성자
    public PlayerData()
    {
        state = State.Alive;
        hp = hp_max;
        facing = 1f;
        move = false;
        ground = false;

        flower_y = new FlowerData(FlowerColor.Yellow, 0);
        flower_b = new FlowerData(FlowerColor.Blue, 0);
        flower_r = new FlowerData(FlowerColor.Red, 0);
    }

    //초기화
    public void Init()
    {
        facing = 1f;
        move = false;
        ground = false;

        flower_current = null;
        flower_y.SetLife(0);
        flower_b.SetLife(0);
        flower_r.SetLife(0);
    }
}
