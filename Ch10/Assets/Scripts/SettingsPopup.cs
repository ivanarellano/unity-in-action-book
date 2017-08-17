using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour {

    [SerializeField] private AudioClip clickSound;

	public void OnSoundToggle() {
        Managers.Audio.SoundMute = !Managers.Audio.SoundMute;
        Managers.Audio.PlaySound(clickSound);
    }

    public void OnSoundValue(float value) {
        Managers.Audio.SoundVolume = value;
    }

    public void OnMusicToggle() {
        Managers.Audio.MusicMute = !Managers.Audio.MusicMute;
        Managers.Audio.PlaySound(clickSound);
    }

    public void OnMusicValue(float value) {
        Managers.Audio.MusicVolume = value;
    }

    public void OnPlayMusic(int selector) {
        Managers.Audio.PlaySound(clickSound);

        switch (selector) {
            case 1:
                Managers.Audio.PlayIntroMusic();
                break;
            case 2:
                Managers.Audio.PlayLevelMusic();
                break;
            default:
                Managers.Audio.StopMusic();
                break;
        }
    }
}
