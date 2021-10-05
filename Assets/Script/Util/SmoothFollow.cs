using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

    //따라갈 대상
    public Transform target;

    //속도값
    private Vector2 velocity;

    //스무스값
    public float smooth_time_x = 0.1f;
    public float smooth_time_y = 0.1f;

    //오프셋
    public float offset_x = 0f;
    public float offset_y = 0f;

    void Start()
    {
        //초기화(아무것도 지정 안해놓을 경우 플레이어로 지정)
        if(target == null)
            target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        //부드럽게 움직이도록 조절
        float posX = Mathf.SmoothDamp(transform.position.x, target.position.x + offset_x, ref velocity.x, smooth_time_x);
        float posY = Mathf.SmoothDamp(transform.position.y, target.position.y + offset_y, ref velocity.y, smooth_time_y);
        float posZ = transform.position.z;

        //조절한 값을 넣어준다.
        transform.position = new Vector3(posX, posY, posZ);
    }
}
