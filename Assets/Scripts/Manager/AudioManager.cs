using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using System.Collections.ObjectModel;

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
        SetupPlayerWalk();  
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

                s.name = s.clip.name;
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
        for (int i = 0; i < sounds.Length; i++)
        {
            Debug.Log("Collection: " + sounds[i].name);
            s = Array.Find(sounds[i].soundsCollection, sound => sound.name == name);
            if (s != null)
            {
                return s;
            }
        }
        return null;
    }

    #region Custom Sound Methods

    AudioSource[] playerWalkSounds;
    float walkTimer = 0f;
    private void SetupPlayerWalk()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == "Player Walk")
            {
                playerWalkSounds = new AudioSource[sounds[i].soundsCollection.Length];
                for (int j = 0; j < sounds[i].soundsCollection.Length; j++)
                {
                    playerWalkSounds[j] = sounds[i].soundsCollection[j].source;
                }
            }
        }
    }

    AudioSource GetRandomWalkSfx()
    {
        return playerWalkSounds[Random.Range(0, playerWalkSounds.Length)];
    }

    public void PlayWalkSFX()
    {
        walkTimer += Time.deltaTime;
        if (walkTimer >= 0.3f)
        {
            walkTimer = 0f;
            AudioSource source =  GetRandomWalkSfx();
            Debug.Log("Walk Audio: " + source.clip.name);
            source.Play();
        }
    }

    #endregion
}
