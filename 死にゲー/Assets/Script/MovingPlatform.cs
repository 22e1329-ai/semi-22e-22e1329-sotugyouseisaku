using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // 右、上など
    public float moveDistance = 3f;               // 移動距離
    public float speed = 1f;                      // 速度

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        // 0〜1を往復
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = startPos + moveDirection.normalized * moveDistance * t;
    }
}
