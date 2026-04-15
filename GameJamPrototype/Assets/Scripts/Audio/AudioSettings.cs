using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [Space]
    [SerializeField] private TextMeshProUGUI musicSliderText;
    [SerializeField] private TextMeshProUGUI sfxSliderText;

    void Start()
    {
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        musicSlider.onValueChanged.AddListener(UpdateMusicTextUI);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXTextUI);

        UpdateMusicTextUI(musicSlider.value);
        UpdateSFXTextUI(sfxSlider.value);
    }

    private void UpdateMusicTextUI(float value)
    {
        var musicValue = Mathf.RoundToInt(musicSlider.value * 100f);
        musicSliderText.text = musicValue.ToString() + "%";
    }
    private void UpdateSFXTextUI(float value)
    {
        var sfxValue = Mathf.RoundToInt(sfxSlider.value * 100f);
        sfxSliderText.text = sfxValue.ToString() + "%";
    }

    void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetSFXVolume);
    }
}
