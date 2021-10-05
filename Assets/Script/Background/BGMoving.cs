using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMoving : MonoBehaviour {

    //참조
    private Transform player;

    //값
    public enum Direction
    {
        Left, Right
    }
    public Direction direction = Direction.Right;
    public float MoveSpeed = 0.1f;
    public float MaxMove = 10.0f;

    //초기값
    private float player_init_x;

	//초기화
	void Start () {
        player = MainPlayer.instance.transform;
        //플레이어 초기 위치
        player_init_x = player.position.x;
    }
	
	//업데이트
	void Update () {
        UpdatePosition();
    }

    //위치 계산
    private void UpdatePosition()
    {
        //현재 위치 - 초기 위치
        float delta_x = player.position.x - player_init_x;
        Vector3 result = transform.localPosition;

        //방향따라 처리
        switch (direction)
        {
            case Direction.Right: result.x = delta_x * MoveSpeed; break;
            case Direction.Left: result.x = delta_x * -MoveSpeed; break;
        }

        //최대 / 최소 값 처리
        result.x = Mathf.Clamp(result.x, -MoveSpeed * MaxMove, MoveSpeed * MaxMove);

        transform.localPosition = result;
    }
}
