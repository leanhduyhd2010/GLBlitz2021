using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip afterDeathBackground;
    public AudioClip playingBackground;

    public AudioClip maceOnPlatform;
    public AudioClip maceOnPlayer;
    public AudioClip sawOnPlatform;
    public AudioClip sawOnPlayer;
    public AudioClip spikeOnPlatform;
    public AudioClip spikeOnPlayer;
    public AudioClip squareMaceOnPlatform;
    public AudioClip squareMaceOnPlayer;

    public AudioClip playerOnScoreBonus;
    public AudioClip playerOnHealthBonus;
    public AudioClip playerOnHit;
    public AudioClip playerFalling;

    public AudioClip healthBonusOnPlayer;
    public AudioClip scoreBonusOnPlayer;
    public AudioClip speedBoostOnPlayer;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance);
    }

    GameObject backgroundSound;

    private void PlaySound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    private void PlayBackgroundSound(AudioClip clip)
    {
        if (backgroundSound == null)
        {
            backgroundSound = new GameObject("backgroundSound");
            backgroundSound.AddComponent<AudioSource>();
        }

        AudioSource audioSource = backgroundSound.GetComponent<AudioSource>();
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
        
    }


    public void PlayPlayerOnHitSound() { PlaySound(playerOnHit); }
    public void PlayMaceOnPlayerSound() { PlaySound(maceOnPlayer); }
    public void PlayMaceOnPlatformSound() { PlaySound(maceOnPlatform); }
    public void PlaySawOnPlayerSound() { PlaySound(sawOnPlayer); }
    public void PlaySawOnPlatformSound() { PlaySound(sawOnPlatform); }
    public void PlaySpikeOnPlayerSound() { PlaySound(spikeOnPlayer); }
    public void PlaySpikeOnPlatformSound() { PlaySound(spikeOnPlatform); }
    public void PlaySquareMaceOnPlayerSound() { PlaySound(squareMaceOnPlayer); }
    public void PlaySquareMaceOnPlatformSound() { PlaySound(squareMaceOnPlatform); }
    public void PlayOnGetScoreBonusSound()
    {
        PlaySound(scoreBonusOnPlayer);
        PlaySound(playerOnScoreBonus);
    }
    public void PlayOnGetHealthBonusSound()
    {
        PlaySound(healthBonusOnPlayer);
        PlaySound(playerOnHealthBonus);
    }
    public void PlayOnGetSpeedBoostSound()
    {
        PlaySound(speedBoostOnPlayer);
    }
    public void PlayPlayerFallingSound() { PlaySound(playerFalling); }
    public void PlayAfterDeathBackgroundSound() { PlayBackgroundSound(afterDeathBackground); }
    public void PlayPlayingBackgroundSound() { PlayBackgroundSound(playingBackground); }
    
}
