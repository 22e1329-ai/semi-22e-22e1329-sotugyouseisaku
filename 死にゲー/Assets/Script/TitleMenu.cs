using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [Header("Scene")]
    [Tooltip("ゲーム本編のシーン名")]
    public string gameSceneName = "GameScene";

    [Header("Audio")]
    [Tooltip("タイトルBGMのAudioSource（任意）")]
    public AudioSource bgmSource;

    // =========================
    // ゲーム開始
    // =========================
    public void StartGame()
    {
        // タイトルBGM停止
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        // ゲームシーンへ
        SceneManager.LoadScene(gameSceneName);
    }

    // =========================
    // ゲーム終了
    // =========================
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Unityエディタ上では再生停止
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後はアプリ終了
        Application.Quit();
#endif
    }
}
