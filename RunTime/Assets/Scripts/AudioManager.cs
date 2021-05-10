using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public GameManager GM;

    public AudioSource BGAS, EffedtAS;
    public AudioClip CoinSnd;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RefreshSoundState()
    {
        if (GM.IsSound)
            BGAS.UnPause();
        else
            BGAS.Pause();
    }

    public void PlayCoinEffect()
    {
        if (GM.IsSound)
            EffedtAS.PlayOneShot(CoinSnd);
    }
}
