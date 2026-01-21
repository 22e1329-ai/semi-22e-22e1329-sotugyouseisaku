using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint; // StartPoint を入れる

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // respawnPointが未設定なら、名前で探す（保険）
        if (respawnPoint == null)
        {
            GameObject sp = GameObject.Find("StartPoint");
            if (sp != null) respawnPoint = sp.transform;
        }
    }

    public void Respawn()
    {
        if (respawnPoint == null) return;

        // 速度リセット（これが超大事）
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // 位置を戻す
        transform.position = respawnPoint.position;
    }
}
