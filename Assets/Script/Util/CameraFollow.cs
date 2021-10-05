using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    //참조
    private Transform player;                   //플레이어 transform값

    //속도값
    private Vector2 velocity;

    //스무스값
    public float smooth_time_x = 0.05f;
    public float smooth_time_y = 0.05f;

    //경계 적용 여부
    public bool bounds;         //경계 적용 여부

    //경계 위치
    public Vector3 min_camera_pos;   //경계 최소값
    public Vector3 max_camera_pos;   //경계 최대값

    void Start()
    {
        //초기화
        player = FindObjectOfType<MainPlayer>().transform;
        //transform.position = player.position;
    }

    void FixedUpdate()
    {
        //카메라 부드럽게 움직이도록 조절
        float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smooth_time_x);
        float posY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smooth_time_y);
        float posZ = transform.position.z;

        //조절한 값을 넣어준다.
        transform.position = new Vector3(posX, posY, posZ);

        //경계 설정
        if (bounds)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, min_camera_pos.x, max_camera_pos.x),
                Mathf.Clamp(transform.position.y, min_camera_pos.y, max_camera_pos.y),
                Mathf.Clamp(transform.position.z, min_camera_pos.z, max_camera_pos.z));
        }
    }
}
