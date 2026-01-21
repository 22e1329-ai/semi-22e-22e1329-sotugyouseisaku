using System.Collections;
using UnityEngine;

public class SpikeUpDown : MonoBehaviour
{
    [Header("Spike")]
    public Transform spike;              // 針本体
    public Collider2D spikeCollider;     // 針の当たり判定

    [Header("Movement (Y only)")]
    public float riseHeight = 1.0f;      // どれだけ上に出るか
    public float riseTime = 0.25f;       // 出る時間
    public float stayTime = 0.8f;        // 出ている時間
    public float returnTime = 0.25f;     // 戻る時間

    public bool triggerOnce = false;

    private Vector3 startLocalPos;
    private bool running = false;
    private bool used = false;

    void Start()
    {
        if (spike == null) return;

        startLocalPos = spike.localPosition;

        if (spikeCollider == null)
            spikeCollider = spike.GetComponent<Collider2D>();

        // 初期は当たり判定OFF
        if (spikeCollider != null)
            spikeCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (running) return;
        if (triggerOnce && used) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpikeRoutine());
        }
    }

    IEnumerator SpikeRoutine()
    {
        running = true;
        used = true;

        Vector3 upPos = startLocalPos + Vector3.up * riseHeight;

        // せり上がり
        yield return MoveY(startLocalPos, upPos, riseTime);

        if (spikeCollider != null)
            spikeCollider.enabled = true;

        // 出ている時間
        yield return new WaitForSeconds(stayTime);

        // 戻る前に判定OFF
        if (spikeCollider != null)
            spikeCollider.enabled = false;

        // 戻る
        yield return MoveY(upPos, startLocalPos, returnTime);

        running = false;
    }

    IEnumerator MoveY(Vector3 from, Vector3 to, float time)
    {
        if (time <= 0f)
        {
            spike.localPosition = to;
            yield break;
        }

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / time);
            a = a * a * (3f - 2f * a); // SmoothStep
            spike.localPosition = Vector3.Lerp(from, to, a);
            yield return null;
        }

        spike.localPosition = to;
    }
}
