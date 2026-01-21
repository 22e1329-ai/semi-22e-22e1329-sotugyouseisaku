using UnityEngine;

public class MovingAxe : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // 右・上など
    public float moveDistance = 3f;               // 動く距離
    public float speed = 2f;                      // 動く速さ

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // -1 ～ 1 を往復
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // 移動
        transform.position = startPos + moveDirection.normalized * moveDistance * t;
    }
}
