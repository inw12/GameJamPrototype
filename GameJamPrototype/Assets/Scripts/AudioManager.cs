using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [Space]
    [SerializeField] private AudioLibrary musicLibrary;
    [SerializeField] private AudioLibrary sfxLibrary;
    [Space]
    public float musicVolume;
    public float sfxVolume;

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

        // Volume
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // Intro Song
        // ** DELETE LATER **
        PlayMusic(musicLibrary.GetClip("IntroA"));
    }

    #region *-- Music ------------------------------*
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
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
        sfxSource.pitch = entry.pitch;
        sfxSource.PlayOneShot(entry.audioClip, entry.volume);
    }
    public void PlaySFXAt(string name, Vector2 position)
    {
        var entry = sfxLibrary.GetEntry(name);
        if (!entry.audioClip) return;
        AudioSource.PlayClipAtPoint(entry.audioClip, position, entry.volume);
    }
    #endregion


    #region *-- Volume ------------------------------*
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MusicVolume, LinearToDecibel(value));
    }
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(SFXVolume, LinearToDecibel(value));
    }
    private float LinearToDecibel(float linear)
    {
        return linear > 0.0001f ? Mathf.Log10(linear) * 20f : -80f;
    }
    #endregion
}
