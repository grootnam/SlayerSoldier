using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip MainMusic, AttackSound, ReloadSound, JumpSound, SwordSound, SwordSound2;
    AudioSource MyAudio;
    public static SoundManager Instance;
    public bool isPlaying;

    // Start is called before the first frame update

    void Awake()
    {
        if (SoundManager.Instance == null)
            SoundManager.Instance = this;
    }

    void Start()
    {
        MyAudio = GetComponent<AudioSource>();
        isPlaying = false;
    }

    public void PlayMainMusic()
    {
        MyAudio.clip = MainMusic;
        MyAudio.loop = true;
        MyAudio.Play();
    }

    public void PlayAttackSound()
    {
        MyAudio.PlayOneShot(AttackSound);
    }

    public void PlayReloadSound()
    {
        MyAudio.PlayOneShot(ReloadSound);
    }

    public void PlayerJumpSound()
    {
        MyAudio.PlayOneShot(JumpSound);
    }

    public void PlaySwordSound()
    {
        MyAudio.PlayOneShot(SwordSound);
    }

    public void PlaySwordSound2()
    {
        MyAudio.PlayOneShot(SwordSound2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}