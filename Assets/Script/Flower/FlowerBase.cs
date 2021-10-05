using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class FlowerBase : MonoBehaviour
{
    //데이터
    public FlowerColor flower_color = FlowerColor.Yellow;
    private PlayerData player_data;

    //에코 - Delay초 마다 빛남
    private EchoSystem echo_system;
    private Transform echo_pos;
    public float delay = 3f;
    private const float timer_offset = 1.0f;
    private Timer timer = new Timer(0, 6f);

    //수명
    public int life = 6;

    //닿으면 사라질지 여부
    public bool destroy = true;

    //초기화
    void Start()
    {
        player_data = DataManager.player_data;
        echo_system = EchoSystem.instance;
        echo_pos = transform.GetChild(0);

        //랜덤으로 빛나게
        float time_limit = delay + Random.Range(-timer_offset, timer_offset);
        float start_time = Random.Range(0, time_limit);
        timer = new Timer(start_time, time_limit);
    }

    //업데이트
    void Update()
    {
        if (timer.AutoTimer())
        {
            echo_system.Echo(flower_color, echo_pos.position, false);
        }
    }

    //플레이어에 닿으면 데이터 넘겨주고 사라짐 - bool값에 따라서 안사라 질수도..
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_data.SetFlowerLife(flower_color, life);
            if(destroy)
                Destroy(gameObject);
        }
    }

    //플레이어에 닿으면 데이터 넘겨주고 사라짐 - bool값에 따라서 안사라 질수도..
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_data.SetFlowerLife(flower_color, life);
            if (destroy)
                Destroy(gameObject);
        }
    }
}