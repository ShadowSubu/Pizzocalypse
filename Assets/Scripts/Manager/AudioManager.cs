using UnityEngine.Audio;
using UnityEngine;
using UnityEditor.SceneManagement;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public SoundCollection[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        AddAudioSources();
    }

    void AddAudioSources()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            for (int j = 0; j < sounds[i].soundsCollection.Length; j++)
            {
                Sound s = sounds[i].soundsCollection[j];
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
    }

    public void Play(string name)
    {
        Sound s = FindSoundInArray(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " Not found");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = FindSoundInArray(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " Not found");
            return;
        }
        s.source.Stop();
    }

    public void PlayOneShot(string name)
    {
        Sound s = FindSoundInArray(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " Not found");
            return;
        }
        s.source.PlayOneShot(s.source.clip);
    }

    public Sound FindSoundInArray(string name)
    {
        Sound s = new Sound();
        foreach (SoundCollection collection in sounds)
        {
            return Array.Find(collection.soundsCollection, sound => sound.name == name);
        }
        return null;
    }
}
