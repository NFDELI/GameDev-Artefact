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

    public AudioClip FireballShotSound;

    public AudioClip SuperAttackSound;

    // Attack Hit Sounds.
    public AudioClip LightHitSound;
    public AudioClip MediumHitSound;
    public AudioClip HeavyHitSound;
    public AudioClip HeavySecondHitSound;

    public AudioClip FireballHitSound;

    public AudioClip SuperHitSound;
    public AudioClip SuperHitSoundEnding;

    // Attack Block Sounds.
    public AudioClip BlockAttackSound;
    public AudioClip ParryAttackSound;
    public AudioClip ParryRyuVoiceSound;
    public AudioClip ParryBossVoiceSound;

    public AudioClip FireballBlockSound;
    public AudioClip FireballBossVoice;

    public AudioClip FellOnGroundSound;

    public AudioClip EvilRyuPostureBreakVoice;
    public AudioClip RyuPostureBreakVoice;

    public AudioClip ArmourSound;

    public AudioClip RyuFireballVoice;
    public AudioClip RyuDragonPunchVoice;
    public AudioClip EvilRyuDragonPunchVoice;

    public AudioClip RyuShinVoice;
    public AudioClip RyuSuperShoryuken;

    public AudioClip EvilRyuTeleportVoice;
    public AudioClip EvilRyuTeleportSound;

    // Intro/Transition Voices.
    public AudioClip RyuIntroductionVoice1;
    public AudioClip RyuIntroductionVoice2;
    public AudioClip EvilRyuIntroductionVoice;

    public AudioClip EvilRyuTransitionVoice;
    public AudioClip RyuTransitionVoice;

    public AudioClip RyuLoseVoice;
    public AudioClip EvilRyuLoseVoice1;
    public AudioClip EvilRyuLoseVoice2;

    public AudioClip EvilRyuWinVoice;

    public AudioClip UnblockableWarningSound;
    public AudioClip SuperBarReadySound;
    public AudioClip SuperBarNotReadySound;

    public AudioClip EvilRyuSuperChargingVoice;
    public AudioClip EvilRyuSuperFireballShootVoice;

    public AudioClip GroundShakeSound;

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
            case 8:
                PlayParryAttackSound();
                break;
            case 9:
                PlayParryRyuVoiceSound();
                break;
            case 10:
                PlayParryBossVoiceSound();
                break;
            case 11:
                PlayFireballBlockSound();
                break;
            case 12:
                PlayFireballHitSound();
                break;
            case 13:
                PlaySuperHitSound();
                break;
            case 14:
                PlaySuperHitSoundEnding();
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

    public void PlayParryAttackSound()
    {
        source.PlayOneShot(ParryAttackSound);
    }

    public void PlayParryRyuVoiceSound()
    {
        source.PlayOneShot(ParryRyuVoiceSound);
    }

    public void PlayParryBossVoiceSound()
    {
        source.PlayOneShot(ParryBossVoiceSound);
    }

    public void PlayFireballShotSound()
    {
        source.PlayOneShot(FireballShotSound);
    }

    public void PlayFireballHitSound()
    {
        source.PlayOneShot(FireballHitSound);
    }

    public void PlayFireballBlockSound()
    {
        source.PlayOneShot(FireballBlockSound);
    }

    public void PlayFellToGroundSound()
    {
        source.PlayOneShot(FellOnGroundSound);
    }

    public void PlayPlayerPostureBreakVoice()
    {
        source.PlayOneShot(RyuPostureBreakVoice);
    }

    public void PlayBossPostureBreakVoice()
    {
        source.PlayOneShot(EvilRyuPostureBreakVoice);
    }

    public void PlayBossFireballVoice()
    {
        source.PlayOneShot(FireballBossVoice);
    }

    public void PlayArmorSound()
    {
        source.PlayOneShot(ArmourSound);
    }

    public void PlayRyuFireballVoice()
    {
        source.PlayOneShot(RyuFireballVoice);
    }

    public void PlayRyuDragonPunchVoice()
    {
        source.PlayOneShot(RyuDragonPunchVoice);
    }

    public void PlayEvilRyuDragonPunchVoice()
    {
        source.PlayOneShot(EvilRyuDragonPunchVoice);
    }

    public void PlayerEvilRyuTeleportSound()
    {
        source.PlayOneShot(EvilRyuTeleportSound);
    }

    public void PlayerEvilRyuTeleportVoice()
    {
        source.PlayOneShot(EvilRyuTeleportVoice);
    }

    public void PlayRyuIntroductionVoice()
    {
        int choice = Random.Range(0, 2);
        if(choice == 0)
        {
            source.PlayOneShot(RyuIntroductionVoice1);
        }
        else if (choice == 1) 
        {
            source.PlayOneShot(RyuIntroductionVoice2);
        }
    }

    public void PlayEvilRyuIntroductionVoice()
    {
        source.PlayOneShot(EvilRyuIntroductionVoice);
    }

    public void PlayEvilRyuTransitionVoice()
    {
        source.PlayOneShot(EvilRyuTransitionVoice);
    }

    public void PlayRyuLoseVoice()
    {
        source.PlayOneShot(RyuLoseVoice);
    }

    public void PlayEvilRyuLoseVoice()
    {
        source.PlayOneShot(EvilRyuLoseVoice1);
    }

    public void PlayEvilRyuLoseVoice2()
    {
        source.PlayOneShot(EvilRyuLoseVoice2);
    }

    public void PlayEvilRyuWinVoice()
    {
        source.PlayOneShot(EvilRyuWinVoice);
    }

    public void PlayUnblockableWarningSound()
    {
        source.PlayOneShot(UnblockableWarningSound);
    }

    public void PlayRyuTransitionVoice()
    {
        source.PlayOneShot(RyuTransitionVoice);
    }

    public void PlayRyuShinVoice()
    {
        source.PlayOneShot(RyuShinVoice);
    }

    public void PlayRyuSuperShoryukenVoice()
    {
        source.PlayOneShot(RyuSuperShoryuken);
    }

    public void PlaySuperHitSound()
    {
        source.PlayOneShot(SuperHitSound);
    }

    public void PlaySuperHitSoundEnding()
    {
        source.PlayOneShot(SuperHitSoundEnding);
    }

    public void PlaySuperAttackSound()
    {
        source.PlayOneShot(SuperAttackSound);
    }

    public void PlaySuperBarReadySound()
    {
        source.PlayOneShot(SuperBarReadySound);
    }

    public void PlaySuperBarNotReadySound()
    {
        source.PlayOneShot(SuperBarNotReadySound);
    }

    public void PlayEvilRyuSuperChargingVoice()
    {
        source.PlayOneShot(EvilRyuSuperChargingVoice);
    }

    public void PlayEvilRyuSuperFireballShootVoice()
    {
        source.PlayOneShot(EvilRyuSuperFireballShootVoice);
    }

    public void PlayGroundShakeSound()
    {
        source.PlayOneShot(GroundShakeSound);
    }

}
