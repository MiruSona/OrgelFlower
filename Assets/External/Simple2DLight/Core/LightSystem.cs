using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple2DLight
{
    public class LightSystem
    {
        //쉐이더 처리할 메터리얼(Shader Material)
        private Material material;

        //최대/최소값(Min / Max)
        public const float size_min = 0.0f;
        public const float size_max = 30.0f;
        public const float bright_min = 0.0f;
        public const float bright_max = 2.0f;
        public const float grow_min = 0.01f;
        public const float grow_max = 100.0f;

        //오프셋
        public const float size_offset = 0.1f;
        public const float grow_offset = 0.001f;
        public const float time_offset = 2.0f;

        //최대 밝기
        private float max_bright = 1.0f;
        private float size_global = 1.0f;

        //보통 조명(Normal Light)
        public struct NormalLight
        {
            public Vector2 position;    //위치
            public float size;          //크기
            public float bright;        //밝기
            public bool on_off;         //켜짐여부
        }
        //커졌다 작아지는 조명(This Light size up and down by timeflow)
        public struct FadeLight
        {
            public Vector2 position;
            public float size;      //현재 크기
            public float size_max;  //최대크기
            public bool size_up;    //커지는 여부
            public float grow_up;     //커지는 속도
            public float grow_down;     //커지는 속도
            public float bright;    //밝기
            public bool on_off;     //켜짐 여부
        }

        //라이트 관련 변수들(Variables)
        private int normal_num = 40;
        private NormalLight[] normal_light = null;
        private int fade_num = 20;
        private FadeLight[] fade_light = null;

        #region 초기화
        //무조건 메터리얼 넘겨받아야되므로 이건 못쓰게만듬
        private LightSystem() { }
        public LightSystem(Material _mat)
        {
            material = _mat;
            InitMaterial();
        }
        public LightSystem(Material _mat, int _normal_light_num, int _fade_light_num, float _max_bright)
        {
            material = _mat;
            normal_num = _normal_light_num;
            fade_num = _fade_light_num;
            max_bright = _max_bright;
            InitMaterial();
        }

        //메터리얼 초기화
        private void InitMaterial()
        {
            //화면 비율 설정
            material.SetFloat("_Ratio", (float)Screen.width / (float)Screen.height);
            //라이트 갯수 설정
            material.SetInt("_NormalLightNum", normal_num);
            material.SetInt("_FadeLightNum", fade_num);
            //최대 밝기 설정
            material.SetFloat("_MaxBright", max_bright);
            //기본값 설정
            normal_light = new NormalLight[normal_num];
            fade_light = new FadeLight[fade_num];
            ApplyNormalLight();
            ApplyFadeLight();
            //모두 꺼짐 표시
            for (int i = 0; i < normal_light.Length; i++)
                normal_light[i].on_off = false;
            //모두 꺼짐 표시
            for (int i = 0; i < fade_light.Length; i++)
                fade_light[i].on_off = false;
        }
        #endregion

        //조명 갱신
        public void UpdateLight()
        {
            ApplyNormalLight();
            UpdateFadeLight();
        }

        //설정값
        public void SetGlobalSize(float _value)
        {
            size_global = _value;
        }

        #region Normal 조명
        //NormalLight 생성
        public int CreateNormalLight(Vector2 _pos, float _size, float _bright)
        {
            int empty_index = -1;
            //비어있는 인덱스 찾기(꽉찼으면 empty_index = -1)
            for (int i = 0; i < normal_num; i++)
            {
                if (!normal_light[i].on_off)
                {
                    empty_index = i;
                    break;
                }
            }

            //만약 데이터 배열이 남아있다면 값 대입
            if (empty_index != -1)
            {
                //켜졌다 표시
                normal_light[empty_index].on_off = true;

                //위치값
                normal_light[empty_index].position = _pos;

                //크기 보정
                normal_light[empty_index].size = LimitValue(_size * size_offset, size_min, size_max);

                //밝기 보정
                normal_light[empty_index].bright = LimitValue(_bright, bright_min, bright_max);

                //메터리얼에 적용
                ApplyNormalLight();
            }

            //인덱스 보내기
            return empty_index;
        }

        //NormalLight 갱신
        public void UpdateNormalLight(int _index, Vector2 _pos, float _size, float _bright)
        {
            //인덱스 확인
            if (0 <= _index && _index < normal_light.Length)
            {
                //해당 조명이 켜졌나 확인
                if (normal_light[_index].on_off)
                {
                    //위치값
                    normal_light[_index].position = _pos;

                    //크기 보정
                    normal_light[_index].size = LimitValue(_size * size_offset, size_min, size_max);

                    //밝기 보정
                    normal_light[_index].bright = LimitValue(_bright, bright_min, bright_max);
                }
            }
        }

        //NormalLight 삭제
        public int DeleteNormalLight(int _index)
        {
            //인덱스 확인
            if (0 <= _index && _index < normal_light.Length)
            {
                //해당 조명이 켜졌나 확인
                if (normal_light[_index].on_off)
                {
                    //모든 값 0으로
                    normal_light[_index].position = Vector2.zero;
                    normal_light[_index].size = 0;
                    normal_light[_index].bright = 0;

                    //조명 끄기
                    normal_light[_index].on_off = false;
                }
            }

            //없다 표시
            return -1;
        }
        #endregion

        #region Fade조명

        //FadeLight 생성
        public void CreateFadeLight(Vector2 _pos, float _size_max, float _grow_up_time, float _grow_down_time, float _bright)
        {
            int empty_index = -1;
            //비어있는 인덱스 찾기(꽉찼으면 empty_index = -1)
            for (int i = 0; i < fade_num; i++)
            {
                if (!fade_light[i].on_off)
                {
                    empty_index = i;
                    break;
                }
            }

            //만약 데이터 배열이 남아있다면
            if (empty_index != -1)
            {
                //켜짐표시
                fade_light[empty_index].on_off = true;
                //위치값
                fade_light[empty_index].position = _pos;
                //크기값
                fade_light[empty_index].size = 0;
                //크기 커진다 표시
                fade_light[empty_index].size_up = true;
                //크기 보정
                fade_light[empty_index].size_max = LimitValue(_size_max * size_offset, size_min, size_max);

                //밝기 보정
                fade_light[empty_index].bright = LimitValue(_bright, bright_min, bright_max);

                //시간 보정
                if (_grow_up_time == 0f)
                    _grow_up_time = 0.01f;
                if (_grow_down_time == 0f)
                    _grow_down_time = 0.01f;
                fade_light[empty_index].grow_up = LimitValue((_size_max / _grow_up_time * time_offset), grow_min, grow_max);
                fade_light[empty_index].grow_down = LimitValue((_size_max / _grow_down_time * time_offset), grow_min, grow_max);
                fade_light[empty_index].grow_up *= grow_offset;
                fade_light[empty_index].grow_down *= grow_offset;

                //메터리얼에 적용
                ApplyFadeLight();
            }
        }

        //FadeLight 갱신
        private void UpdateFadeLight()
        {
            //조명 전체 갱신
            for (int i = 0; i < fade_light.Length; i++)
            {
                //조명 켜졌나 확인(밝기 != 0)
                if (fade_light[i].on_off)
                {
                    //커지는 중인 조명이면
                    if (fade_light[i].size_up)
                    {
                        //다 커질때까지++
                        if (fade_light[i].size + fade_light[i].grow_up < fade_light[i].size_max)
                            fade_light[i].size += fade_light[i].grow_up;
                        else
                            fade_light[i].size_up = false;
                    }
                    //작아지는 중인 조명이면
                    else
                    {
                        //다 작아질때까지--
                        if (fade_light[i].size - fade_light[i].grow_down > 0)
                            fade_light[i].size -= fade_light[i].grow_down;
                        //다 작아지면 0으로 처리 + 꺼짐 표시
                        else
                        {
                            fade_light[i].size = 0;
                            fade_light[i].bright = 0;
                            fade_light[i].on_off = false;
                        }
                    }
                }
            }

            //메터리얼에 적용
            ApplyFadeLight();
        }
        #endregion

        #region 값 보정
        //값 제한
        private float LimitValue(float _value, float _min, float _max)
        {
            float value = _value;
            if (value < _min)
                value = _min;
            else if (value > _max)
                value = _max;

            return value;
        }
        #endregion

        #region 메터리얼 처리
        private void ApplyNormalLight()
        {
            Vector4[] apply_data = new Vector4[normal_num];

            for (int i = 0; i < apply_data.Length; i++)
            {
                //켜져있으면 값 적용
                if (normal_light[i].on_off)
                {
                    Vector2 pos = Camera.main.WorldToViewportPoint(normal_light[i].position);
                    apply_data[i].x = pos.x;
                    apply_data[i].y = pos.y;
                    apply_data[i].z = normal_light[i].size * size_global;
                    apply_data[i].w = normal_light[i].bright;
                }
                //꺼져있으면 전부 0으로
                else
                    apply_data[i] = Vector4.zero;
            }

            material.SetVectorArray("_NormalLights", apply_data);
        }

        private void ApplyFadeLight()
        {
            Vector4[] apply_data = new Vector4[fade_num];

            for (int i = 0; i < apply_data.Length; i++)
            {
                //켜져있으면 값 적용
                if (fade_light[i].on_off)
                {
                    Vector2 pos = Camera.main.WorldToViewportPoint(fade_light[i].position);
                    apply_data[i].x = pos.x;
                    apply_data[i].y = pos.y;
                    apply_data[i].z = fade_light[i].size * size_global;
                    apply_data[i].w = fade_light[i].bright;
                }
                //꺼져있으면 전부 0으로
                else
                    apply_data[i] = Vector4.zero;
            }

            material.SetVectorArray("_FadeLights", apply_data);
        }
        #endregion
    }
}
