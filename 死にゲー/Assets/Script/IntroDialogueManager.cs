using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroDialogueManager : MonoBehaviour
{
    [Serializable]
    public class Line
    {
        public string speaker;
        [TextArea(2, 5)] public string text;
    }

    [Header("UI")]
    public GameObject dialogueCanvas;   // DialogueCanvas
    public TMP_Text nameText;           // NameText
    public TMP_Text bodyText;           // BodyText
    public Button nextButton;           // NextButton（なくても可）

    [Header("Dialogue")]
    public Line[] lines;

    private int index = 0;
    private Action onFinished;

    void Awake()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(Next);
    }

    public void Play(Action finishedCallback)
    {
        onFinished = finishedCallback;

        index = 0;
        dialogueCanvas.SetActive(true);
        ShowLine();

        // Spaceでも進めたい場合
        //（Updateで受ける）
        enabled = true;
    }

    void Update()
    {
        // 会話中だけ：Space / Enter で進む
        if (dialogueCanvas != null && dialogueCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Next();
            }
        }
    }

    void ShowLine()
    {
        if (lines == null || lines.Length == 0)
        {
            Finish();
            return;
        }

        index = Mathf.Clamp(index, 0, lines.Length - 1);

        if (nameText != null) nameText.text = lines[index].speaker;
        if (bodyText != null) bodyText.text = lines[index].text;
    }

    public void Next()
    {
        if (lines == null || lines.Length == 0) { Finish(); return; }

        index++;
        if (index >= lines.Length)
        {
            Finish();
            return;
        }
        ShowLine();
    }

    void Finish()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        enabled = false;

        onFinished?.Invoke();
        onFinished = null;
    }
}
