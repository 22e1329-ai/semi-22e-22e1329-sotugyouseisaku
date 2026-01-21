using UnityEngine;

public class GoalBGMTrigger : MonoBehaviour
{
    public AudioSource bgmSource;     // GameBGMのAudioSource
    public AudioClip goalBGM;         // ゴール用BGM

    private bool changed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (changed) return;

        if (other.CompareTag("Player"))
        {
            changed = true;

            bgmSource.clip = goalBGM;
            bgmSource.Play();
        }
    }
}
