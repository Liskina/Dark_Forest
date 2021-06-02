using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip getCoinSound, playerHitSound, playerFallSound, 
        getPotionSound, pauseSound, yesSound, vocalSound, jumpSound;

    static AudioSource audioSource;

    void Start()
    {
        getCoinSound = Resources.Load<AudioClip>("getCoin");
        playerHitSound = Resources.Load<AudioClip>("playerHit");
        playerFallSound = Resources.Load<AudioClip>("playerFall");
        getPotionSound = Resources.Load<AudioClip>("getPotion");
        pauseSound = Resources.Load<AudioClip>("pause");
        yesSound = Resources.Load<AudioClip>("yes");
        jumpSound = Resources.Load<AudioClip>("jump");
  

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "getCoin":
                audioSource.PlayOneShot(getCoinSound);
                break;
            case "playerHit":
                audioSource.PlayOneShot(playerHitSound);
                break;
            case "playerFall":
                audioSource.PlayOneShot(playerFallSound);
                break;
            case "getPotion":
                audioSource.PlayOneShot(getPotionSound);
                break;
            case "pause":
                audioSource.PlayOneShot(pauseSound);
                break;
            case "yes":
                audioSource.PlayOneShot(yesSound);
                break;
            case "jump":
                audioSource.PlayOneShot(jumpSound);
                break;
           
        }
    }
}
