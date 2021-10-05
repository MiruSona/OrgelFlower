using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    //참조
    private List<GameObject> particles = new List<GameObject>();

    //값
    private int particle_num = 10;

	//초기화
	void Start () {
        for(int i = 0; i < particle_num; i++)
            particles.Add(GetComponentInChildren<ParticleSystem>().gameObject);
    }
	
	//파티클 실행
	public void PlayParticle(Vector3 _pos)
    {
        for(int i = 0; i < particle_num; i++)
        {
            if (!particles[i].activeSelf)
            {
                particles[i].transform.position = _pos;
                particles[i].SetActive(true);
                break;
            }
        }
    }
}
