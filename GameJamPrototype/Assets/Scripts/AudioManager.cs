using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource musicSource;
    [Space]
    [SerializeField] private AudioLibrary musicLibrary;
    [SerializeField] private AudioLibrary sfxLibrary;
    [Space]
    [SerializeField][Range(0f, 1f)] private float defaultVolume = 0.5f;

    private const string MusicVolume = "MusicVolume";
    private const string SFXVolume = "SFXVolume";

    void Awake()    // Singleton Initialization
    {
        // Singleton
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Volume
        SetMusicVolume(defaultVolume);
        SetSFXVolume(defaultVolume);

        // Intro Song
        // ** DELETE LATER **
        PlayMusic("IntroA");
    }


    #region *-- Music ------------------------------*
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    public void PlayMusic(string clipName, bool loop = true)
    {
        AudioClip clip = musicLibrary.GetClip(clipName);
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    public void StopMusic() => musicSource.Stop();
    #endregion


    #region *-- SFX -----------------------------------*
    public void PlaySFX(string name)
    {
        var entry = sfxLibrary.GetEntry(name);
        if (!entry.audioClip) return;
        PlaySFXAt(name, Camera.main.transform.position);
    }

    public void PlaySFXAt(string name, Vector2 position)
    {
        var entry = sfxLibrary.GetEntry(name);
        if (!entry.audioClip) return;
        AudioSource.PlayClipAtPoint(entry.audioClip, position, entry.volume);
    }
    #endregion


    #region *-- Volume ------------------------------*
    // Setters
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MusicVolume, LinearToDecibel(value));
    }
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(SFXVolume, LinearToDecibel(value));
    }

    // Getters
    public float GetMusicVolume()
    {
        mixer.GetFloat(MusicVolume, out float volume);
        return DecibalToLinear(volume);
    }
    public float GetSFXVolume()
    {
        mixer.GetFloat(SFXVolume, out float volume);
        return DecibalToLinear(volume);
    }

    // Helper Functions
    private float LinearToDecibel(float linear) => linear > 0.0001f ? Mathf.Log10(linear) * 20f : -80f;
    private float DecibalToLinear(float decibal) => decibal > -80f ? Mathf.Pow(10f, decibal / 20f) : 0.0001f;
    #endregion
}
