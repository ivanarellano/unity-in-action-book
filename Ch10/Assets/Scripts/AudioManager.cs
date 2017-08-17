using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager {

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;
    [SerializeField] private string introBgMusic;
    [SerializeField] private string levelBgMusic;

    private AudioSource activeMusic;
    private AudioSource inactiveMusic;

    public ManagerStatus status { get; private set; }

    public float crossFadeRate = 1.5f;
    private bool isCrossfading;

    private float musicVolume;
    public float MusicVolume {
        get { return musicVolume; }
        set {
            musicVolume = value;

            if (music1Source != null && !isCrossfading) {
                music1Source.volume = musicVolume;
                music2Source.volume = musicVolume;
            }
        }
    }

    public bool MusicMute {
        get {
            if (music1Source != null) {
                return music1Source.mute;
            }
            return false;
        }
        set {
            if (music1Source != null) {
                music1Source.mute = value;
            }
            if (music2Source != null) {
                music2Source.mute = value;
            }
        }
    }

    public float SoundVolume {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value;}
    }

    public bool SoundMute {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    private void PlayMusic(AudioClip clip) {
        if (isCrossfading) return;
        StartCoroutine(CrossFadeMusic(clip));
    }

    private AudioClip GetMusicResource(string fileName) {
        return Resources.Load("Music/" + fileName) as AudioClip;
    }

    private IEnumerator CrossFadeMusic(AudioClip clip) {
        isCrossfading = true;

        /// Prepare playing clip at lowest volume
        inactiveMusic.clip = clip;
        inactiveMusic.volume = 0;
        inactiveMusic.Play();

        float scaledRate = crossFadeRate * musicVolume;
        while (activeMusic.volume > 0) {
            activeMusic.volume -= scaledRate * Time.deltaTime;
            inactiveMusic.volume += scaledRate * Time.deltaTime;

            yield return null;
        }

        AudioSource temp = activeMusic; /// Swap active and inactive

        activeMusic = inactiveMusic;
        activeMusic.volume = musicVolume;

        inactiveMusic = temp;
        inactiveMusic.Stop();

        isCrossfading = false;
    }

    public void Startup() {
        Debug.Log("Audio manager starting...");

        SoundVolume = 1f;
        MusicVolume = 1f;

        activeMusic = music1Source;
        inactiveMusic = music2Source;

        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;

        status = ManagerStatus.Started;
    }

    public void PlaySound(AudioClip clip) {
        soundSource.PlayOneShot(clip);
    }

    public void PlayIntroMusic() {
        PlayMusic(GetMusicResource(introBgMusic));
    }

    public void PlayLevelMusic() {
        PlayMusic(GetMusicResource(levelBgMusic));
    }

    public void StopMusic() {
        music1Source.Stop();
    }
}
