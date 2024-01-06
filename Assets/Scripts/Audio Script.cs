using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip JumpSound;
    public AudioClip LightAttackSound;
    public AudioClip MediumAttackSound;

    public void PlayJumpSound()
    {
        source.PlayOneShot(JumpSound);
    }
    
    public void PlayLightAttackSound()
    {
        source.PlayOneShot(LightAttackSound);
    }

    public void PlayerMediumAttackSound()
    {
        source.PlayOneShot(MediumAttackSound);
    }
}
