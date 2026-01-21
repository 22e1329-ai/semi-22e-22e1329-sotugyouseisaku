using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TimeLimitManager : MonoBehaviour
{
    [Header("Limit Settings")]
    public float timeLimitSeconds = 7f * 60f;   // 7分
    public string titleSceneName = "TitleScene";

    [Header("UI")]
    public TMP_Text limitTimeText;   // 残り時間
    public TMP_Text timeUpText;      // TIME UP 表示

    private float remaining;
    private bool running = true;
    private bool timeUpTriggered = false;

    void Start()
    {
        remaining = timeLimitSeconds;
        UpdateUI();

        if (timeUpText != null)
            timeUpText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!running || timeUpTriggered) return;

        remaining -= Time.deltaTime;

        if (remaining <= 0f)
        {
            remaining = 0f;
            UpdateUI();
            StartCoroutine(TimeUpRoutine());
            return;
        }

        UpdateUI();
    }

    IEnumerator TimeUpRoutine()
    {
        timeUpTriggered = true;
        running = false;

        // TIME UP 表示
        if (timeUpText != null)
            timeUpText.gameObject.SetActive(true);

        // 3秒待つ（ポーズ中でも進む）
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }

    void UpdateUI()
    {
        if (limitTimeText == null) return;

        int total = Mathf.CeilToInt(remaining);
        int min = total / 60;
        int sec = total % 60;

        limitTimeText.text = $"{min:00}:{sec:00}";
    }

    // クリア時に呼ぶ用（任意）
    public void StopLimit()
    {
        running = false;
    }
}
