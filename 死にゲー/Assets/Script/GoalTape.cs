using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTape : MonoBehaviour
{
    [Header("Scene")]
    public string endSceneName = "EndScene";

    private bool cleared = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (cleared) return;

        if (other.CompareTag("Player"))
        {
            cleared = true;

            // 必要ならここでSE鳴らす、操作停止、タイム停止なども可能
            SceneManager.LoadScene(endSceneName);
        }
    }
}
