using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple2DLight
{
    public class FadeLight : MonoBehaviour
    {
        //컴포넌트
        private LightManager light_manager;

        //조명
        public float light_size = 3f;
        public float light_grow_up_time = 2.5f;
        public float light_grow_down_time = 2f;
        public float light_bright = 1f;

        //초기화
        void Start()
        {
            light_manager = LightManager.instance;
        }
        
        //조명 On
        public void Lighting()
        {
            //쉐이더 On
            light_manager.CreateFadeLight(
                transform.position,
                light_size,
                light_grow_up_time,
                light_grow_down_time,
                light_bright
                );
        }
    }
}
