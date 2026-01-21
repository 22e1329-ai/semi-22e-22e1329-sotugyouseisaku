using UnityEngine;

public class GameStartController : MonoBehaviour
{
    [Header("References")]
    public PlayerJumpKing player;                 // あなたのプレイヤー
    public IntroDialogueManager introDialogue;    // 会話マネージャ
    public TimeLimitManager timeLimit;            // 制限時間（あれば）

    void Start()
    {
        // 1) 開始時は操作禁止
        if (player != null) player.canControl = false;

        // 2) 制限時間も止める（会話後に開始させたい場合）
        if (timeLimit != null)
        {
            // TimeLimitManager側に Startで動き出す実装なら、
            // runningをfalseで止められるようにしておくのが理想。
            // ここでは簡単に「スクリプト無効化」で止める。
            timeLimit.enabled = false;
        }

        // 3) 会話開始 → 終わったらゲーム開始
        if (introDialogue != null)
        {
            introDialogue.Play(OnIntroFinished);
        }
        else
        {
            // 会話が無いなら普通に開始
            OnIntroFinished();
        }
    }

    void OnIntroFinished()
    {
        // 操作解除
        if (player != null) player.canControl = true;

        // 制限時間開始（スクリプトを再有効化）
        if (timeLimit != null) timeLimit.enabled = true;
    }
}
