using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    private AudioSource _audio;
    public static SoundManager instance
    {
        get{return _instance;}
    }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void Jouer(AudioClip clip, float volume = 1f)
    {
        _audio.PlayOneShot(clip, volume);
    }
}