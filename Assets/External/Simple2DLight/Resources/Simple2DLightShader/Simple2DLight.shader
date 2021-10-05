Shader "Custom/Simple2DLight" {
	Properties {
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
	}
	
	SubShader{
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			//화면 받아오는 텍스쳐
			uniform sampler2D _MainTex;

			//조명 갯수(Normal = 고정 크기 조명 / Fade = 커졌다 작아지는 조명)
			int _NormalLightNum; int _FadeLightNum;
			//조명값 -> xy = 위치, z = 크기, w = 빛 밝기( 0 = 꺼짐)
			fixed4 _NormalLights[40]; fixed4 _FadeLights[20];
			//화면비(16: 9 이런거..)
			fixed _Ratio;
			//최대밝기
			fixed _MaxBright;

			//화면 처리(조명 처리)
			fixed4 frag(v2f_img i) : COLOR{
				//화면 칼라값 받아옴 + 창 비율값 받아옴
				fixed4 color = tex2D(_MainTex, i.uv);
				fixed2 ratio = fixed2(1, 1 / _Ratio);

				//조명 Smooth하게 만들기 위한 변수들
				fixed delta = 0;
				fixed ray = 0;

				//조명 생성
				int index = 0;
				//Normal Light
				for (index = 0; index < _NormalLightNum; index++) {
					//길이 계산(Vector길이 계산) -> 길이((위치 - UV값) * 화면비))
					ray = length((_NormalLights[index].xy - i.uv.xy) * ratio);
					//보간값 계산
					delta += smoothstep(_NormalLights[index].z, 0, ray) * _NormalLights[index].w;
				}
				//Fade Light
				for (index = 0; index < _FadeLightNum; index++) {
					//길이 계산(Vector길이 계산) -> 길이((위치 - UV값) * 화면비))
					ray = length((_FadeLights[index].xy - i.uv.xy) * ratio);
					//보간값 계산
					delta += smoothstep(_FadeLights[index].z, 0, ray) * _FadeLights[index].w;
				}

				//최대 밝기 제한
				if (delta > _MaxBright)
					delta = _MaxBright;

				//화면에 적용
				color.rgb *= delta;
				return color;
			}
			ENDCG
		}
	}	
}
