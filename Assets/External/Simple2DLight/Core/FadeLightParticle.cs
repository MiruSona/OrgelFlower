using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple2DLight
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FadeLightParticle : MonoBehaviour
    {
        //컴포넌트
        private LightManager light_manager;
        private ParticleSystem particle_system;

        //조명
        public float light_size = 3f;
        public float light_grow_up_time = 1f;
        public float light_grow_down_time = 2f;
        public float light_bright = 1f;

        //초기값
        private float init_size = 3f;
        private float init_grow_up_time = 1f;
        private float init_grow_down_time = 2f;

        //오프셋
        private const float particle_size_offset = 2.5f;

        //초기화
        void Awake()
        {
            light_manager = LightManager.instance;
            particle_system = transform.GetComponentInChildren<ParticleSystem>();
            SetParticle(light_size, light_grow_up_time, light_grow_down_time);

            init_size = light_size;
            init_grow_up_time = light_grow_up_time;
            init_grow_down_time = light_grow_down_time;
        }

        //외부 초기화
        public void Init()
        {
            light_size = init_size;
            light_grow_up_time = init_grow_up_time;
            light_grow_down_time = init_grow_down_time;

            SetParticle(light_size, light_grow_up_time, light_grow_down_time);
        }
        public void Init(float _size)
        {
            light_size = _size;

            SetParticle(light_size, light_grow_up_time, light_grow_down_time);
        }
        public void Init(float _size, float _grow_up_time, float _grow_down_time)
        {
            light_size = _size;
            light_grow_up_time = _grow_up_time;
            light_grow_down_time = _grow_down_time;

            SetParticle(light_size, light_grow_up_time, light_grow_down_time);
        }

        //쉐이더에 맞춰서 파티클 조절
        public void SetParticle(float _size, float _grow_up, float _grow_down)
        {
            //파티클 멈춤
            particle_system.Stop();

            //파티클 받아오기
            ParticleSystem.MainModule particle = particle_system.main;

            //시간 조절
            float duration = _grow_up + _grow_down - particle.startLifetime.constant;
            if (duration < 0.1f)
                duration = 0.1f;
            particle.duration = duration;

            //크기 조절
            float size = _size * particle_size_offset;
            particle.startSize = new ParticleSystem.MinMaxCurve(size);
        }

        //조명 On
        public void Lighting()
        {
            //FadeLight 생성
            light_manager.CreateFadeLight(
                transform.position,
                light_size,
                light_grow_up_time,
                light_grow_down_time,
                light_bright
                );

            //파티클 Play
            particle_system.Play();
        }
    }
}
