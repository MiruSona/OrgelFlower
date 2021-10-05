using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

namespace Simple2DLight
{
    [RequireComponent(typeof(Camera))]
    public class LightManager : MonoBehaviour
    {
        //쉐이더 처리할 Material(Shader Material)
        private Material material;

        //쉐이더 메터리얼(조명) 처리하는 객체(This Handling Lights)
        private LightSystem light_system;

        //조명 갯수(Light num) -> 최대 100개(Max Num = 100)
        public int NormalLightNum = 40;
        public int FadeLightNum = 20;

        //최대 밝기
        public float MaxBright = 1.0f;

        //조명 갯수 최대/최소
        private const int light_max = 100;
        private const int light_min = 0;
        
        //초기화(Init)
        private void Awake()
        {
            //메터리얼 생성(Create Material)
            material = Resources.Load<Material>("Simple2DLightShader/SLShader");

            //조명 갯수 체크
            int normal_num = -1, fade_num = -1;
            if (light_min <= NormalLightNum && NormalLightNum <= light_max)
                normal_num = NormalLightNum;
            if (light_min <= FadeLightNum && FadeLightNum <= light_max)
                fade_num = FadeLightNum;

            //메터리얼 생성됬나 체크
            if (material != null)
            {
                //조명갯수를 잘못 입력한 경우 기본값으로 생성
                if (normal_num == -1 || fade_num == -1)
                    light_system = new LightSystem(material);
                //조명값 다 올바르게 입력한 경우 원하는 갯수대로 생성
                else
                    light_system = new LightSystem(material, normal_num, fade_num, MaxBright);
            }
        }

        //조명 갱신 실행
        private void Start()
        {
            StartCoroutine(UpdateLight());
        }

        #region 조명 처리(Light Update)

        //조명 갱신(Update Light)
        private IEnumerator UpdateLight()
        {
            while(material != null)
            {
                light_system.UpdateLight();
                yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
            }
        }

        //쉐이더 적용(Apply Shader)
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //이 함수가 소스(화면)을 가져와 후처리(matrial을 소스에 적용)한 후 Destination(후처리한 화면)로 게임화면에 적용
            //쉽게말해서 화면을 가져와 material로 후처리를 하고 후처리한 화면을 게임에 보여줌
            if (material != null)
                Graphics.Blit(source, destination, material);
        }
        #endregion

        #region 호출 함수(Call Function)

        #region NormalLight
        //보통 조명 생성(Create Normal Light)
        //size max = 0 ~ 30f, bright = 0 ~ 2f
        public void CreateNormalLight(Vector2 _position, float _size, float _bright)
        {
            light_system.CreateNormalLight(_position, _size, _bright);
        }
        //인덱스를 넘겨주는 경우
        public void CreateNormalLight(ref int _index, Vector2 _position, float _size, float _bright)
        {
            _index = light_system.CreateNormalLight(_position, _size, _bright);
        }

        //보통 조명 갱신(Update Normal Light)
        //size max = 0 ~ 30f, bright = 0 ~ 2f
        public void UpdateNormalLight(int _index, Vector2 _position, float _size, float _bright)
        {
            light_system.UpdateNormalLight(_index, _position, _size, _bright);
        }

        //보통 조명 삭제(Delete Normal Light)
        public void DeleteNormalLight(ref int _index)
        {
            _index = light_system.DeleteNormalLight(_index);
        }
        #endregion

        #region FadeLight
        //커졌다 작아지는 조명 생성(Create Fade Light)
        //커지는 & 작아지는데 걸리는 시간이 같은 경우
        //size max = 0 ~ 30f, grow_time = 0.01f ~ 100f,  bright = 0 ~ 2f
        public void CreateFadeLight(Vector2 _position, float _size_max, float _grow_time, float _bright)
        {
            light_system.CreateFadeLight(_position, _size_max, _grow_time, _grow_time, _bright);
        }

        //커지는 & 작아지는데 걸리는 시간이 다른 경우
        //size max = 0 ~ 30f, grow_up_time & grow_down_time = 0.01f ~ 100f,  bright = 0 ~ 2f
        public void CreateFadeLight(Vector2 _position, float _size_max, float _grow_up_time, float _grow_down_time, float _bright)
        {
            light_system.CreateFadeLight(_position, _size_max, _grow_up_time, _grow_down_time, _bright);
        }
        #endregion

        //글로벌 크기 설정
        public void SetGlobalSize(float _value)
        {
            light_system.SetGlobalSize(_value);
        }

        #endregion

        #region 싱글톤 처리(SingleTon)
        private static LightManager _instance;
        public static LightManager instance
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
                        _instance = FindObjectOfType<LightManager>();
                    }
                    catch (System.Exception e)
                    {
                        print(e.StackTrace);
                        return null;
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}
