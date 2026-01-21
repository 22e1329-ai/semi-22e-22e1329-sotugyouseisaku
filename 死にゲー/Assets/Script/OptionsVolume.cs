using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string volumeParam = "MasterVolume";
    public Slider volumeSlider;

    private const string SaveKey = "MASTER_VOL";

    void Start()
    {
        float v = PlayerPrefs.GetFloat(SaveKey, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0.0001f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = v;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        SetVolume(v);
    }

    public void SetVolume(float value01)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value01, 0.0001f, 1f)) * 20f;
        if (mixer != null) mixer.SetFloat(volumeParam, dB);

        PlayerPrefs.SetFloat(SaveKey, value01);
        PlayerPrefs.Save();
    }
}
