using System;
using UnityEngine;
[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [Serializable]
    public struct AudioEntry
    {
        public string name;
        public AudioClip audioClip;
        [Range(0f, 1f)]     public float volume;
        [Range(0.5f, 1.5f)] public float pitch;
    }

    public AudioEntry[] audio;

    public AudioClip GetClip(string name)
    {
        foreach (var entry in audio)
            if (entry.name == name) return entry.audioClip;

        return null;
    }

    public AudioEntry GetEntry(string name)
    {
        foreach (var entry in audio)
            if (entry.name == name) return entry;

        return default;
    }
}
