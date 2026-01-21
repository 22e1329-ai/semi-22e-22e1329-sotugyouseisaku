using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsRoll : MonoBehaviour
{
    [Header("UI")]
    public RectTransform creditsContainer; // 上に動かす親
    public float speed = 60f;             // スクロール速度（px/秒）

    [Header("Finish")]
    public float endY = 3000f;             // ここまで行ったら終了
    public string nextScene = "TitleScene";// 終了後に戻す先

    void Update()
    {
        if (creditsContainer == null) return;

        // 上へスクロール
        creditsContainer.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // 一定位置まで行ったらタイトルへ（任意）
        if (creditsContainer.anchoredPosition.y >= endY)
        {
            SceneManager.LoadScene(nextScene);
        }

        // Escでスキップ（任意）
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
