using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour {

    //참조
    public GameObject tutorial;

    //실행 여부
    private bool do_show = false;

	//초기화
	void Start () {
        tutorial.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //플레이어 들어오면 튜토리얼 보이기
        if (collision.CompareTag("Player") && !do_show)
        {
            tutorial.SetActive(true);
            do_show = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어 나가면 튜토리얼 끄기
        if (collision.CompareTag("Player") && tutorial.activeSelf)
        {
            tutorial.SetActive(false);
        }
    }
}
