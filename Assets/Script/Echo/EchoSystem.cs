using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class EchoSystem : SingleTon<EchoSystem> {

    //데이터
    private const string echo_path = "Prefab/Echo/";
    private EchoEffect[] yellow_echo, blue_echo, red_echo, none_echo;

    //갯수
    private const int echo_max = 20;

    //초기화
    private void Start()
    {
        InitEchos();
    }
    
    //Echo 배열 미리 생성 + 꺼둠
    private void InitEchos()
    {
        EchoEffect yellow_effect = Resources.Load<EchoEffect>(echo_path + "YellowEcho");
        EchoEffect blue_effect = Resources.Load<EchoEffect>(echo_path + "BlueEcho");
        EchoEffect red_effect = Resources.Load<EchoEffect>(echo_path + "RedEcho");
        EchoEffect none_effect = Resources.Load<EchoEffect>(echo_path + "NoneEcho");

        yellow_echo = new EchoEffect[echo_max];
        blue_echo = new EchoEffect[echo_max];
        red_echo = new EchoEffect[echo_max];
        none_echo = new EchoEffect[echo_max];
        for (int i = 0; i < echo_max; i++)
        {
            yellow_echo[i] = Instantiate(yellow_effect);
            yellow_echo[i].transform.parent = transform;
            yellow_echo[i].gameObject.SetActive(false);

            blue_echo[i] = Instantiate(blue_effect);
            blue_echo[i].transform.parent = transform;
            blue_echo[i].gameObject.SetActive(false);

            red_echo[i] = Instantiate(red_effect);
            red_echo[i].transform.parent = transform;
            red_echo[i].gameObject.SetActive(false);

            none_echo[i] = Instantiate(none_effect);
            none_echo[i].transform.parent = transform;
            none_echo[i].gameObject.SetActive(false);
        }
    }

    //이펙트 생성
    public void Echo(FlowerColor _color, Vector3 _position, bool _col_onoff)
    {
        EchoEffect[] echo_list = null;
        switch (_color)
        {
            case FlowerColor.Yellow: echo_list = yellow_echo; break;
            case FlowerColor.Blue: echo_list = blue_echo; break;
            case FlowerColor.Red: echo_list = red_echo; break;
            case FlowerColor.None: echo_list = none_echo; break;
        }
        //선택한게 있다면 에코!
        if(echo_list != null)
            EchoingOne(echo_list, _position, _col_onoff);
    }

    //이펙트 생성 + 크기
    public void Echo(FlowerColor _color, Vector3 _position, float _size, bool _col_onoff)
    {
        EchoEffect[] echo_list = null;
        switch (_color)
        {
            case FlowerColor.Yellow: echo_list = yellow_echo; break;
            case FlowerColor.Blue: echo_list = blue_echo; break;
            case FlowerColor.Red: echo_list = red_echo; break;
            case FlowerColor.None: echo_list = none_echo; break;
        }
        //선택한게 있다면 에코!
        if (echo_list != null)
            EchoingOne(echo_list, _position, _size, _col_onoff);
    }

    //이펙트 생성 + 전부 조절
    public void Echo(FlowerColor _color, Vector3 _position, float _size, float _grow_up_time, float _grow_down_time, bool _col_onoff)
    {
        EchoEffect[] echo_list = null;
        switch (_color)
        {
            case FlowerColor.Yellow: echo_list = yellow_echo; break;
            case FlowerColor.Blue: echo_list = blue_echo; break;
            case FlowerColor.Red: echo_list = red_echo; break;
            case FlowerColor.None: echo_list = none_echo; break;
        }
        //선택한게 있다면 에코!
        if (echo_list != null)
            EchoingOne(echo_list, _position, _size, _grow_up_time, _grow_down_time, _col_onoff);
    }

    //리스트에서 하나만 에코
    private void EchoingOne(EchoEffect[] _echo_list, Vector3 _position, bool _col_onoff)
    {
        foreach(EchoEffect echo in _echo_list)
        {
            //안켜진애 찾음
            if (!echo.gameObject.activeSelf)
            {
                //에코!
                echo.Echo(_position, _col_onoff);
                break;
            }
        }
    }

    private void EchoingOne(EchoEffect[] _echo_list, Vector3 _position, float _size, bool _col_onoff)
    {
        foreach (EchoEffect echo in _echo_list)
        {
            //안켜진애 찾음
            if (!echo.gameObject.activeSelf)
            {
                //에코!
                echo.Echo(_position, _size, _col_onoff);
                break;
            }
        }
    }

    private void EchoingOne(EchoEffect[] _echo_list, Vector3 _position, float _size, float _grow_up_time, float _grow_down_time, bool _col_onoff)
    {
        foreach (EchoEffect echo in _echo_list)
        {
            //안켜진애 찾음
            if (!echo.gameObject.activeSelf)
            {
                //에코!
                echo.Echo(_position, _size, _grow_up_time, _grow_down_time, _col_onoff);
                break;
            }
        }
    }
}
