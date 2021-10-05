using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class FlowerData {
    
    //꽃 색
    public readonly FlowerColor flower_color = FlowerColor.None;

    //꽃의 수명(몇 회 기능할지)
    private int life_max = -1;
    private int life_current = -1;
    public const int life_max_offset = 100;       //최대값
    public const int life_min_offset = 0;         //최소값

    //스프라이트
    private const string sprite_path = "Sprite/Flower/FlowerLeaf/";
    private const int leaf_num = 6;
    private Sprite[] leaf_sprite = new Sprite[leaf_num];
    
    #region 초기화(생성자)
    //기본 생성자 막아둠
    private FlowerData() { }
    //꽃 색 + 수명
    public FlowerData(FlowerColor _color, int _life)
    {
        flower_color = _color;
        if(_life < 0)
            SetLife(0);
        else
            SetLife(_life);
        InitSprite(_color);
    }
    //Sprite초기화
    public void InitSprite(FlowerColor _color)
    {
        string name = "";
        switch (_color)
        {
            case FlowerColor.Yellow: name = "YellowFlower"; break;
            case FlowerColor.Blue: name = "BlueFlower"; break;
            case FlowerColor.Red: name = "RedFlower"; break;
        }
        if (_color != FlowerColor.None)
        {
            for(int i = 0; i < leaf_num; i++)
                leaf_sprite[i] = Resources.Load<Sprite>(sprite_path + name + (i+1));
        }    
    }
    #endregion
    
    #region 수명
    //수명 Get/Set
    public void SetLife(int _life)
    {
        if (life_min_offset <= _life && _life <= life_max_offset)
            life_max = _life;

        life_current = life_max;
    }
    public int GetLife()
    {
        return life_current;
    }

    //수명++
    public void AddLife(int _value)
    {
        if ((life_current + _value) < life_max)
            life_current += _value;
    }

    //수명--
    public void SubLife(int _value)
    {
        if ((life_current - _value) > life_min_offset)
            life_current -= _value;
        else
            life_current = 0;
    }
    public void SubLife()
    {
        if ((life_current - 1) > life_min_offset)
            life_current--;
        else
            life_current = 0;
    }

    //수명 남았나 체크
    public bool CheckLife()
    {
        //남았나 체크
        if (life_current > life_min_offset)
            return true;
        else
            return false;
    }
    #endregion

    #region Sprite 관련
    //전부 넘기기
    public Sprite[] GetLeafSprite()
    {
        return leaf_sprite;
    }

    //남은 수명에 따라 보내기
    public Sprite GetLeafSpriteByLife()
    {
        //인덱스 구하기
        int index = 0;
        float length = leaf_sprite.Length - 1;  //배열이라 길이 - 1
        float current = life_current;           //현재 수명
        float max = life_max;                   //최대 수명

        //배열 최대값 - 배열길이 * (현재 / 최대 수명)
        index = (leaf_sprite.Length - 1) - (int)Mathf.Floor((length - 1) * (current / max));
        if (index > 0)
            index--;

        //현재 수명에 맞는 스프라이트 보내기
        return leaf_sprite[index];
    }
    #endregion

}
