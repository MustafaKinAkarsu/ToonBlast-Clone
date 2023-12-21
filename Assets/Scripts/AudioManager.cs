using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource _audioSource;
    [SerializeField] AudioClip[] audioClips;

    public static AudioManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public void PlaySound(int i)
    {
        _audioSource.clip = audioClips[i];
        _audioSource.Play();
    }
   
}
