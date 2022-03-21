using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public List<Sound> sounds;
    public List<Sound> dialogues;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void Play(string name)
    {
        Sound sound = sounds.Find(sound => sound.name == name);
        if (sound != null)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.audioClip;

            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;

            audioSource.Play();
            Destroy(audioSource, 2f);
        }
        else
            Debug.LogWarning($"Sound {sound.name} not found.");
    }

    public void PlayDialogue(string name)
    {
        Sound sound = dialogues.Find(sound => sound.name == name);
        if (sound != null)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.audioClip;

            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;

            audioSource.Play();
            Destroy(audioSource, 15f);
        }
        else
            Debug.LogWarning($"Sound {sound.name} not found.");
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)] public float volume = .5f;
    [Range(-3f, 3f)] public float pitch = 1f;

    public bool loop;
}