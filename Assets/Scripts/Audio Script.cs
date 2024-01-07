using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip JumpSound;

    // Attack Swing Sounds.
    public AudioClip LightAttackSound;
    public AudioClip MediumAttackSound;
    public AudioClip HeavyAttackSound;

    // Attack Hit Sounds.
    public AudioClip LightHitSound;
    public AudioClip MediumHitSound;
    public AudioClip HeavyHitSound;
    public AudioClip HeavySecondHitSound;

    // Attack Block Sounds.
    public AudioClip BlockAttackSound;

    public void PlayJumpSound()
    {
        source.PlayOneShot(JumpSound);
    }
    
    public void SoundIndexPlay(int index)
    {
        switch (index) 
        { 
            case 0:
                PlayLightAttackSound();
                break;
            case 1:
                PlayMediumAttackSound();
                break;
            case 2:
                PlayHeavyAttackSound();
                break;
            case 3:
                PlayLightHitSound();
                break;
            case 4:
                PlayMediumHitSound();
                break;
            case 5:
                PlayHeavyHitSound();
                break;
            case 6:
                PlayHeavySecondHitSound();
                break;
            case 7:
                PlayBlockAttackSound();
                break;
            default:
                break;
        }
    }

    public void PlayLightAttackSound()
    {
        source.PlayOneShot(LightAttackSound);
    }

    public void PlayMediumAttackSound()
    {
        source.PlayOneShot(MediumAttackSound);
    }

    public void PlayHeavyAttackSound()
    {
        source.PlayOneShot(HeavyAttackSound);
    }

    public void PlayLightHitSound()
    {
        source.PlayOneShot(LightHitSound);
    }

    public void PlayMediumHitSound()
    {
        source.PlayOneShot(MediumHitSound);
    }

    public void PlayHeavyHitSound() 
    {
        source.PlayOneShot(HeavyHitSound);
    }

    public void PlayHeavySecondHitSound()
    {
        source.PlayOneShot(HeavySecondHitSound);
    }

    public void PlayBlockAttackSound()
    {
        source.PlayOneShot(BlockAttackSound);
    }
}
