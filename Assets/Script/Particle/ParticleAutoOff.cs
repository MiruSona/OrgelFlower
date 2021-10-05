using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoOff : MonoBehaviour {

    //컴포넌트
    private ParticleSystem particle;

	//초기화
	void Start () {
        particle = GetComponent<ParticleSystem>();
    }
	
	//파티클 끝나면 꺼지기
	void Update () {
        if (!particle.isPlaying)
            gameObject.SetActive(false);
	}
}
