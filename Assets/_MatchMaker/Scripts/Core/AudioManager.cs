using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class AudioManager : MonoBehaviour
{
    public enum AudioType
    {
        SFX,
        MUSIC
    }

    [SerializeField] private int defaultPoolSize;
    [SerializeField] private int maxPoolSize;
    private Queue<AudioSource> _audioSources = new Queue<AudioSource>();
    private Dictionary<AudioSource, bool> _playBackState = new Dictionary<AudioSource, bool>();
    public static AudioManager Instance;
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
       
        Init();
    }
    private void Init()
    {
        for (int i = 0; i < defaultPoolSize; i++)
        {
            _audioSources.Enqueue(CreateAudioSource(i));
        }
    }
    private void PauseSources()
    {
        _playBackState.Clear();
        foreach (var audioSource in _audioSources)
        {
            _playBackState.Add(audioSource, audioSource.isPlaying);
            audioSource.Pause();
        }
    }
    private void UnPauseSources()
    {
        foreach (var state in _playBackState)
        {
            if (state.Value)
            {
                state.Key.Play();
            }
        }
    }
    public AudioSource GetSource(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;
        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }

    private AudioSource CreateAudioSource(int i)
    {
        GameObject go = new GameObject("AudioSource" + i);
        go.transform.parent = transform;
        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }

    public AudioSource PlayClip(AudioClip clip, AudioType audioType, bool loop = false, float volume = 1f, bool loopForSeconds = false, float loopDuration = 1f)
    {

        if (clip == null)
            return null;
        AudioSource source = _audioSources.Dequeue();

        if (source.clip)
        {
            if (source.isPlaying)
            {
                _audioSources.Enqueue(source);
                bool foundValidSource = false;
                int counter = 0;
                while (!foundValidSource && counter < _audioSources.Count)
                {
                    source = _audioSources.Dequeue();
                    if (source.clip)
                    {
                        if (source.isPlaying)
                            _audioSources.Enqueue(source);
                        else
                            foundValidSource = true;
                    }
                    ++counter;
                }
                if (!foundValidSource)
                {
                    if (_audioSources.Count >= maxPoolSize)
                    {
                        Debug.Log("Max pool size reached, no more allocation possible");
                        return null;
                    }
                    source = CreateAudioSource(_audioSources.Count);
                }

            }
        }
        source.clip = clip;
        source.Play();
        source.loop = loop;
        source.volume = volume;
        if (loopForSeconds)
        {
            source.loop = true;
            StartCoroutine(DisableLoop(source, loopDuration));
        }
        _audioSources.Enqueue(source);
        return source;
    }
    public void StopClip(AudioClip clip)
    {
        if (clip == null) return;
        foreach (var source in _audioSources)
        {
            if (source && source.isPlaying)
            {
                if (source.clip == clip)
                {
                    source.Stop();
                }
            }
        }
    }
    private IEnumerator DisableLoop(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        source.loop = false;
    }
}
