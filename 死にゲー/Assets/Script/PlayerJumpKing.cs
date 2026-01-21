using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerJumpKing : MonoBehaviour
{
    [Header("Control")]
    public bool canControl = true;   // ★会話中は false にする

    [Header("Jump Settings")]
    public float minJumpForce = 5f;
    public float maxJumpForce = 20f;
    public float chargeTimeLimit = 1f;

    [Header("Air Move")]
    public float airMoveSpeed = 3f;

    [Header("UI")]
    public Slider chargeSlider;
    public TextMeshProUGUI jumpCountText;

    [Header("Visual Sprites")]
    public Sprite idleSprite;
    public Sprite[] chargeSprites;
    public Sprite inAirSprite;

    [Header("SE")]
    public AudioClip chargeLoopSE;   // 溜め中ループ音（任意）
    public AudioClip jumpSE;         // ジャンプ瞬間SE（任意）
    public AudioClip landSE;         // 着地SE（任意）
    [Range(0f, 1f)] public float seVolume = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private bool isCharging = false;
    private float currentCharge = 0f;
    private float chargeTimer = 0f;

    private int jumpCount = 0;

    // 着地判定用
    private bool wasGrounded = true;

    // 向き管理（1=右, -1=左）
    private int facing = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;

        if (chargeSlider != null) chargeSlider.value = 0f;

        UpdateJumpText();
        SetIdleSprite();
    }

    void Update()
    {
        // ★会話中などはここで操作を止める
        if (!canControl)
        {
            // 溜め中だったら強制解除＋SE停止
            if (isCharging)
            {
                isCharging = false;
                StopChargeLoop();
                if (chargeSlider != null) chargeSlider.value = 0f;
                SetIdleSprite();
            }

            // 左右入力で勝手に動かないようにする（速度固定）
            if (rb != null) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            return;
        }

        bool groundedNow = IsGrounded();

        // 着地SE（空中→地面）
        if (!wasGrounded && groundedNow)
        {
            PlayOneShotSafe(landSE);
        }
        wasGrounded = groundedNow;

        // 向き変更（左右反転）
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0)
        {
            facing = (h > 0) ? 1 : -1;
            if (sr != null) sr.flipX = (facing == -1);
        }

        // 溜め開始
        if (Input.GetKeyDown(KeyCode.Space) && groundedNow)
        {
            isCharging = true;
            chargeTimer = 0f;
            currentCharge = minJumpForce;

            if (chargeSlider != null) chargeSlider.value = 0f;

            UpdateChargeSprite(0f);
            StartChargeLoop();
        }

        // 溜め中
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            float t = Mathf.Clamp01(chargeTimer / chargeTimeLimit);
            currentCharge = Mathf.Lerp(minJumpForce, maxJumpForce, t);

            if (chargeSlider != null) chargeSlider.value = t;
            UpdateChargeSprite(t);

            if (chargeTimer >= chargeTimeLimit) Jump();
            if (Input.GetKeyUp(KeyCode.Space)) Jump();
        }

        // 空中左右移動
        if (!groundedNow)
        {
            float move = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(move * airMoveSpeed, rb.linearVelocity.y);

            if (!isCharging) SetInAirSprite();
        }
        else
        {
            if (!isCharging) SetIdleSprite();
        }
    }

    void Jump()
    {
        isCharging = false;
        StopChargeLoop();
        PlayOneShotSafe(jumpSE);

        jumpCount++;
        UpdateJumpText();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * currentCharge, ForceMode2D.Impulse);

        if (chargeSlider != null) chargeSlider.value = 0f;

        SetInAirSprite();
    }

    // 音まわり
    void StartChargeLoop()
    {
        if (audioSource == null || chargeLoopSE == null) return;
        if (audioSource.isPlaying && audioSource.loop && audioSource.clip == chargeLoopSE) return;

        audioSource.loop = true;
        audioSource.clip = chargeLoopSE;
        audioSource.volume = seVolume;
        audioSource.Play();
    }

    void StopChargeLoop()
    {
        if (audioSource == null) return;

        if (audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }

    void PlayOneShotSafe(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip, seVolume);
    }

    // 表示系
    void UpdateJumpText()
    {
        if (jumpCountText != null)
            jumpCountText.text = $"JUMP : {jumpCount}";
    }

    void SetIdleSprite()
    {
        if (sr != null && idleSprite != null)
            sr.sprite = idleSprite;
    }

    void SetInAirSprite()
    {
        if (sr == null) return;

        if (inAirSprite != null) sr.sprite = inAirSprite;
        else if (chargeSprites != null && chargeSprites.Length > 0)
            sr.sprite = chargeSprites[chargeSprites.Length - 1];
    }

    void UpdateChargeSprite(float t01)
    {
        if (sr == null) return;

        if (chargeSprites == null || chargeSprites.Length == 0)
        {
            SetIdleSprite();
            return;
        }

        int idx = Mathf.Clamp(Mathf.FloorToInt(t01 * chargeSprites.Length), 0, chargeSprites.Length - 1);
        sr.sprite = chargeSprites[idx];
    }

    bool IsGrounded()
    {
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }
}
