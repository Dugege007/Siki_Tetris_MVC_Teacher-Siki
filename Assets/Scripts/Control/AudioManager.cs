using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Control ctrl;

    private AudioSource audioSource;

    public AudioClip cursorClip;
    public AudioClip dropClip;
    public AudioClip moveClip;
    public AudioClip rowCleatClip;

    private bool isMute = false;

    private void Awake()
    {
        ctrl = GetComponent<Control>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCursor()
    {
        PlayAudio(cursorClip);
    }

    public void PlayDrop()
    {
        PlayAudio(dropClip);
    }

    public void PlayMove()
    {
        PlayAudio(moveClip);
    }

    public void PlayRowClear()
    {
        PlayAudio(rowCleatClip);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (isMute) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnAudioBtnClick()
    {
        isMute = !isMute;

        if (isMute) // 静音状态
        {
            ctrl.view.SetMuteActive(true);
        }
        else        // 播放状态
        {
            PlayCursor();
            ctrl.view.SetMuteActive(false);
        }
    }
}
