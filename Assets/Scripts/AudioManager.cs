using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class SoundClip
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField]  List<SoundClip> soundClips = new List<SoundClip>();
    public AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        SoundClip clip = soundClips.Find(s => s.name == soundName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip.clip, clip.volume);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }
}