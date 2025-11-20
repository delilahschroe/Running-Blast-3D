using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using CandyCoded.HapticFeedback;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Main Menu Setting")]
    public Sprite onSprite;
    public Sprite offSprite;
    public Button soundBtn;
    public Button musicBtn;
    public Button vibrationBtn;

    [Header("Audio Clip")]
    [Space(10)]
    public AudioClip buttonClickSound;
    public AudioClip trashPickSound;

    [Header("Audio Source")]
    [Space(10)]
    public AudioSource bgSoundSource;
    public AudioSource btnSoundSource;
    private void OnEnable()
    {
        SoundSettingButtonAdjusment();
        if (SaveSystem.SoundOn)
        {
            bgSoundSource.mute = false;
            bgSoundSource.UnPause();
        }
        else
        {
            bgSoundSource.mute = true;
            bgSoundSource.Pause();
        }
    }
    private void Awake()
    {
        instance = this;
        soundBtn.onClick.AddListener(() =>
        {
            SaveSystem.SoundOn = !SaveSystem.SoundOn;
            SoundSettingButtonAdjusment();
            if (SaveSystem.SoundOn)
            {
                bgSoundSource.mute = false;
                bgSoundSource.UnPause();
            }
            else
            {
                bgSoundSource.mute = true;
                bgSoundSource.Pause();
            }
            ButtonClicked();
        });
        musicBtn.onClick.AddListener(() =>
        {
            SaveSystem.MusicOn = !SaveSystem.MusicOn;
            SoundSettingButtonAdjusment();
            if (SaveSystem.MusicOn)
            {
                btnSoundSource.mute = false;
            }
            else
            {
                btnSoundSource.mute = true;
            }
            ButtonClicked();
        });
        vibrationBtn.onClick.AddListener(() =>
        {
            SaveSystem.VibaratinOn = !SaveSystem.VibaratinOn;
            SoundSettingButtonAdjusment();
            ButtonClicked();
        });
    }

    void SoundSettingButtonAdjusment()
    {
        if (SaveSystem.SoundOn)
        {
            soundBtn.GetComponent<Image>().sprite = onSprite;
        }
        else
        {
            soundBtn.GetComponent<Image>().sprite = offSprite;
        }
        if (SaveSystem.MusicOn)
        {
            musicBtn.GetComponent<Image>().sprite = onSprite;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = offSprite;
        }
        if (SaveSystem.VibaratinOn)
        {
            vibrationBtn.GetComponent<Image>().sprite = onSprite;
        }
        else
        {
            vibrationBtn.GetComponent<Image>().sprite = offSprite;
        }
    }
    public void ButtonClicked()
    {
        if (SaveSystem.MusicOn)
        {
            btnSoundSource.PlayOneShot(buttonClickSound);
        }
    }
    public void TrashSoundAndVibration()
    {
        if (SaveSystem.MusicOn)
        {
            btnSoundSource.PlayOneShot(trashPickSound);
        }
        if (SaveSystem.VibaratinOn)
        {
           // HapticFeedback.LightFeedback();
        }
    }
}
