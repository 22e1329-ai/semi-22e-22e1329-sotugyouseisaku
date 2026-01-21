using UnityEngine;

public class KillZone : MonoBehaviour
{
    [Header("SE")]
    public AudioClip hitSE;          // オノに当たった時のSE
    public float seVolume = 1f;

    private AudioSource audioSource;
    private bool triggered = false;  // 多重発火防止

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // ① SEを鳴らす
            if (hitSE != null)
            {
                if (audioSource != null)
                    audioSource.PlayOneShot(hitSE, seVolume);
                else
                    AudioSource.PlayClipAtPoint(hitSE, transform.position, seVolume);
            }

            // ② リスポーン
            PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
            if (pr != null) pr.Respawn();

            // ③ 少し後に多重発火解除（保険）
            Invoke(nameof(ResetTrigger), 0.1f);
        }
    }

    void ResetTrigger()
    {
        triggered = false;
    }
}
