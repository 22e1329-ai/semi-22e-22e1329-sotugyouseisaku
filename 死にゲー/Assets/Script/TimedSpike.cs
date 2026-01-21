using System.Collections;
using UnityEngine;

public class TimedSpike : MonoBehaviour
{
    [Header("Movement (Y only)")]
    public float riseHeight = 1.0f;
    public float riseTime = 0.25f;
    public float stayUpTime = 0.8f;
    public float returnTime = 0.25f;
    public float stayDownTime = 0.8f;

    [Header("Components")]
    public Collider2D spikeCollider;
    public SpriteRenderer spikeRenderer;
    public AudioSource audioSource;

    [Header("Options")]
    public bool colliderOnlyWhenUp = true;
    public bool hideWhenDown = true;

    [Header("SE")]
    public AudioClip popSE;                 // ★出現SE
    [Range(0f, 1f)] public float seVolume = 1f;

    [Header("Start")]
    public float startDelay = 0f;

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;

        // 自動取得（保険）
        if (spikeCollider == null) spikeCollider = GetComponent<Collider2D>();
        if (spikeRenderer == null) spikeRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // 初期状態：引っ込み
        SetVisible(false);

        StartCoroutine(LoopRoutine());
    }

    IEnumerator LoopRoutine()
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        Vector3 upPos = startLocalPos + Vector3.up * riseHeight;

        while (true)
        {
            // 下で待機（見えない）
            SetVisible(false);
            if (stayDownTime > 0f)
                yield return new WaitForSeconds(stayDownTime);

            // ★ 出現開始：見える + SE
            SetVisible(true);
            PlayPopSE();

            // 出る
            yield return MoveLocal(startLocalPos, upPos, riseTime);

            // 上で待機
            if (colliderOnlyWhenUp && spikeCollider != null)
                spikeCollider.enabled = true;

            if (stayUpTime > 0f)
                yield return new WaitForSeconds(stayUpTime);

            // 戻る前に当たり判定OFF
            if (colliderOnlyWhenUp && spikeCollider != null)
                spikeCollider.enabled = false;

            // 戻る（戻り中は見える）
            yield return MoveLocal(upPos, startLocalPos, returnTime);
        }
    }

    IEnumerator MoveLocal(Vector3 from, Vector3 to, float time)
    {
        if (time <= 0f)
        {
            transform.localPosition = to;
            yield break;
        }

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / time);
            a = a * a * (3f - 2f * a); // SmoothStep
            transform.localPosition = Vector3.Lerp(from, to, a);
            yield return null;
        }

        transform.localPosition = to;
    }

    void SetVisible(bool visible)
    {
        if (hideWhenDown && spikeRenderer != null)
            spikeRenderer.enabled = visible;

        if (colliderOnlyWhenUp && spikeCollider != null)
            spikeCollider.enabled = visible;
    }

    void PlayPopSE()
    {
        if (popSE == null || audioSource == null) return;
        audioSource.PlayOneShot(popSE, seVolume);
    }
}
