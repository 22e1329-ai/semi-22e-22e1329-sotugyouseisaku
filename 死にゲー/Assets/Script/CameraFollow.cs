using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // プレイヤー
    public float upFollowSpeed = 4f;    // 上方向追従スピード
    public float downFollowSpeed = 0.3f;// 下方向追従スピード（落下時）
    public float horizontalSpeed = 4f;  // 横方向追従スピード
    public float yOffset = 1f;          // プレイヤーより少し上を見る

    private Vector3 currentPos;

    void Start()
    {
        currentPos = transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 目標位置（カメラの理想位置）
        float targetY = target.position.y + yOffset;
        float targetX = target.position.x;

        // --- Y のジャンプキング式追従 ---
        if (targetY > currentPos.y)
        {
            // 上昇：素早く追う
            currentPos.y = Mathf.Lerp(currentPos.y, targetY, upFollowSpeed * Time.deltaTime);
        }
        else
        {
            // 落下：ゆっくり追う（絶望感を出す）
            currentPos.y = Mathf.Lerp(currentPos.y, targetY, downFollowSpeed * Time.deltaTime);
        }

        // --- X 方向の追従（普通のスムーズさ） ---
        currentPos.x = Mathf.Lerp(currentPos.x, targetX, horizontalSpeed * Time.deltaTime);

        // カメラ更新
        transform.position = new Vector3(currentPos.x, currentPos.y, transform.position.z);
    }
}
