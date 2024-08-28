using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : SingletonBehaviour<AudioManager> {

    public Slider BGMslide;
    public Slider SFXslide;
    public AudioClip[] BGMs;
    public AudioClip[] SFXs;

    public AudioMixer MasterMixer;

    public AudioSource BGMSource;
    public AudioSource SFXSource;

    public AudioMixerSnapshot MainSnap;
    public AudioMixerSnapshot ADSnap;

    float bgmA;
    float sfxB;

    public void Start() {
        PlaySound(0, true);
        GetSound();
        BGMslide.onValueChanged.AddListener(delegate { SetBGM(BGMslide.value); });
        SFXslide.onValueChanged.AddListener(delegate { SetSFX(SFXslide.value); });
    }
    public void SetBGM(float bgm) {
        bgmA = (bgm * 80) - 80;

        MasterMixer.SetFloat("BGM", bgmA);
        PlayerPrefs.SetFloat("BGM", bgmA);
    }
    public void SetSFX(float SFX) {
        sfxB = (SFX * 80) - 80;

        MasterMixer.SetFloat("SFX", sfxB);
        PlayerPrefs.SetFloat("SFX", sfxB);
    }

    public void GetSound() {
        float BGM = PlayerPrefs.GetFloat("BGM", 1.0f);
        MasterMixer.SetFloat("BGM", BGM);
        BGMslide.value = ((BGM + 80f) / 80f);

        float SFX = PlayerPrefs.GetFloat("SFX", 1.0f);
        MasterMixer.SetFloat("SFX", SFX);
        SFXslide.value = ((SFX + 80f) / 80f);
    }

    public void ADPause() {
        ADSnap.TransitionTo(.01f);
    }
    public void RunMainSnap() {
        MainSnap.TransitionTo(.01f);
    }

    public void PlaySound(int id, bool isBGM = false) {
        if (isBGM) {
            if (id < 0 || id > BGMs.Length - 1) {
                return;
            }
            AudioClip clip = BGMs[id];
            BGMSource.clip = clip;
            BGMSource.Play();
        } else {
            if (id < 0 || id > SFXs.Length - 1) {
                return;
            }
            AudioClip clip = SFXs[id];
            SFXSource.clip = clip;
            SFXSource.Play();
        }
    }
}
