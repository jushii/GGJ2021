using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameService
{
    public List<AudioSource> _sfxSources;
    public List<AudioClip> lightPunchClips;
    public List<AudioClip> hardPunchClips;
    public List<AudioClip> hitClips;
    public List<AudioClip> screamClips;

    public void PlayLightPunchSFX()
    {
        AudioSource source = _sfxSources[Random.Range(0, _sfxSources.Count)];
        source.pitch = Random.Range(0.9f, 1.1f);
        AudioClip clip = lightPunchClips[Random.Range(0, lightPunchClips.Count)];
        source.PlayOneShot(clip);
    }
    
    public void PlayHardPunchSFX()
    {
        AudioSource source = _sfxSources[Random.Range(0, _sfxSources.Count)];
        source.pitch = Random.Range(0.9f, 1.1f);
        AudioClip clip = hardPunchClips[Random.Range(0, hardPunchClips.Count)];
        source.PlayOneShot(clip);
    }

    public void PlayHitSFX()
    {
        AudioSource source = _sfxSources[Random.Range(0, _sfxSources.Count)];
        source.pitch = Random.Range(0.9f, 1.1f);
        AudioClip clip = hitClips[Random.Range(0, hitClips.Count)];
        source.PlayOneShot(clip);
    }

    public void PlayScreamSFX()
    {
        AudioSource source = _sfxSources[Random.Range(0, _sfxSources.Count)];
        source.pitch = Random.Range(0.9f, 1.1f);
        AudioClip clip = screamClips[Random.Range(0, screamClips.Count)];
        source.PlayOneShot(clip);
    }
}
