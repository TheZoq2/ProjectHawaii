﻿using UnityEngine;

/// <summary>
///   Audio source that fades between clips instead of playing them immediately.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FadingAudioSource : MonoBehaviour
{
    #region Fields

    /// <summary>
    ///   Volume to end the previous clip at.
    /// </summary>
    public float FadeOutThreshold = 0.05f;

    /// <summary>
    ///   Volume change per second when fading.
    /// </summary>
    public float FadeSpeed = 0.05f;

    /// <summary>
    ///   Actual audio source.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    ///   Whether the audio source is currently fading, in or out.
    /// </summary>
    private FadeState fadeState = FadeState.None;

    /// <summary>
    ///   Next clip to fade to.
    /// </summary>
    private AudioClip nextClip;

    /// <summary>
    ///   Whether to loop the next clip.
    /// </summary>
    private bool nextClipLoop;

    /// <summary>
    ///   Target volume to fade the next clip to.
    /// </summary>
    private float nextClipVolume;

    #endregion

    #region Enums

    public enum FadeState
    {
        None,

        FadingOutNext,

        FadingIn,

        FadingOut
    }

    #endregion

    #region Public Properties

    /// <summary>
    ///   Current clip of the audio source.
    /// </summary>
    public AudioClip Clip
    {
        get
        {
            return audioSource.clip;
        }
    }

    /// <summary>
    ///   Whether the audio source is currently playing a clip.
    /// </summary>
    public bool IsPlaying
    {
        get
        {
            return audioSource.isPlaying;
        }
    }

    /// <summary>
    ///   Whether the audio source is looping the current clip.
    /// </summary>
    public bool Loop
    {
        get
        {
            return audioSource.loop;
        }
    }

    /// <summary>
    ///   Current volume of the audio source.
    /// </summary>
    public float Volume
    {
        get
        {
            return audioSource.volume;
        }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///   If the audio source is enabled and playing, fades out the current clip and fades in the specified one, after.
    ///   If the audio source is enabled and not playing, fades in the specified clip immediately.
    ///   If the audio source is not enalbed, fades in the specified clip as soon as it gets enabled.
    /// </summary>
    /// <param name="clip">Clip to fade in.</param>
    /// <param name="volume">Volume to fade to.</param>
    /// <param name="loop">Whether to loop the new clip, or not.</param>
    public void Fade(AudioClip clip, float volume, bool loop)
    {
        if (clip == null)
        {
            return;
        }

        nextClip = clip;
        nextClipVolume = volume;
        nextClipLoop = loop;

        if (audioSource.enabled)
        {
            if (IsPlaying)
            {
                fadeState = FadeState.FadingOutNext;
            }
            else
            {
                FadeToNextClip();
            }
        }
        else
        {
            FadeToNextClip();
        }
    }

    /// <summary>
    ///   Continues fading in the current audio clip.
    /// </summary>
    public void Play(float volume)
    {
        nextClipVolume = volume;
        fadeState = FadeState.FadingIn;
        audioSource.Play();
    }

    /// <summary>
    ///   Stop playing the current audio clip immediately.
    /// </summary>
    public void Stop()
    {
        audioSource.Stop();
        fadeState = FadeState.None;
    }

    public void StopByFade()
    {
        nextClipVolume = 0;
        fadeState = FadeState.FadingOut;
    }

    #endregion

    #region Methods

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
    }

    private void FadeToNextClip()
    {
        audioSource.clip = nextClip;
        audioSource.loop = nextClipLoop;

        fadeState = FadeState.FadingIn;

        if (audioSource.enabled)
        {
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        audioSource.enabled = false;
        Stop();
    }

    private void OnEnable()
    {
        audioSource.enabled = true;
        Play(audioSource.volume);
    }

    private void Update()
    {
        if (!audioSource.enabled)
        {
            return;
        }

        if (fadeState == FadeState.FadingOut)
        {
            if (audioSource.volume > FadeOutThreshold)
            {
                // Fade out current clip.
                audioSource.volume -= FadeSpeed * Time.deltaTime;
            }
            else
            {
                Stop();
            }
        }
        else if (fadeState == FadeState.FadingOutNext)
        {
            if (audioSource.volume > FadeOutThreshold)
            {
                // Fade out current clip.
                audioSource.volume -= FadeSpeed * Time.deltaTime;
            }
            else
            {
                // Start fading in next clip.
                FadeToNextClip();
            }
        }
        else if (fadeState == FadeState.FadingIn)
        {
            if (audioSource.volume < nextClipVolume)
            {
                // Fade in next clip.
                audioSource.volume += FadeSpeed * Time.deltaTime;
            }
            else
            {
                // Stop fading in.
                fadeState = FadeState.None;
            }
        }
    }

    #endregion
}