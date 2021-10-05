using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple2DLight
{
    public class NormalLight : MonoBehaviour
    {
        //컴포넌트
        private LightManager light_manager;

        //조명
        public float light_size = 3f;
        public float light_bright = 1f;

        //인덱스
        private int index = 0;

        //초기화
        void Awake()
        {
            light_manager = LightManager.instance;
        }

        //조명 생성
        public void CreateLight(Vector2 _position)
        {
            //쉐이더 생성
            light_manager.CreateNormalLight(
                ref index,
                _position,
                light_size,
                light_bright
                );
        }

        //조명 위치 이동
        public void MoveLight(Vector2 _position)
        {
            light_manager.UpdateNormalLight(index, _position, light_size, light_bright);
        }
    }
}