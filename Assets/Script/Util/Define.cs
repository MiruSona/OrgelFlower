using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define {
    
    //타이머
    public class Timer
    {
        //타이머용
        public float time = 0;
        public float limit = 0;
        public bool end = false;

        public Timer(float _time, float _limit)
        {
            time = _time;
            limit = _limit;
        }

        //시간 자동 체크
        public bool AutoTimer()
        {
            end = false;
            if (time < limit)
                time += Time.deltaTime;
            else
            {
                time = 0;
                end = true;
            }
            return end;
        }

        //시간만++
        public void AddTimer()
        {
            if (time < limit)
                time += Time.deltaTime;
        }

        //시간초기화
        public void InitTimer()
        {
            time = 0;
        }

        //시간 됬는지 체크
        public bool CheckTimer()
        {
            end = false;
            if (time >= limit)
            {
                end = true;
            }
            return end;
        }
    }

    //꽃 색
    public enum FlowerColor
    {
        None,
        Yellow,
        Blue,
        Red
    }

    //색 처리
    public static class ColorUtil
    {
        //색 정의
        private static Color white = Color.white;
        private static Color yellow = Color.yellow;
        private static Color blue = Color.blue;
        private static Color red = Color.red;

        //꽃 색에 따라 반환
        public static Color GetFlowerColor(FlowerColor _color)
        {
            Color send = white;
            switch(_color)
            {
                case FlowerColor.Yellow: send = yellow; break;
                case FlowerColor.Blue: send = blue; break;
                case FlowerColor.Red: send = red; break;
                case FlowerColor.None: send = white; break;
            }

            return send;
        }
    }
}
