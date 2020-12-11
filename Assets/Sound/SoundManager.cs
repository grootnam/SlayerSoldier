using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip MainMusic, AttackSound, ReloadSound, JumpSound, SwordSound, SwordSound2, explosionSound,
        EarthquakeSound, BigBallSound, Stage2patternASound, ThrowingSound, fireSound, forceSound, AlertSound1, AlertSound2, MissileSound, MapReduceSound, windSound;
    AudioSource MyAudio;
    AudioSource windaudio;
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
        windaudio=gameObject.AddComponent<AudioSource>();
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

    public void PlayMissile()
    {
        MyAudio.PlayOneShot(MissileSound);
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

    public void Stage1PatternA()
    {
        MyAudio.PlayOneShot(explosionSound);
    }

    public void Stage1PatternB()
    {
        MyAudio.PlayOneShot(BigBallSound);
    }

    public void Stage1PatternC()
    {
        MyAudio.PlayOneShot(EarthquakeSound);
    }
    public void Stage2PatternAsword()
    {
        MyAudio.PlayOneShot(Stage2patternASound);
    }
    public void Stage2PatternAthrowing()
    {
        MyAudio.PlayOneShot(ThrowingSound);
    }
    public void Stage2PatternB()
    {
        MyAudio.PlayOneShot(fireSound);
    }
    public void Stage2PatternC()
    {
        MyAudio.PlayOneShot(fireSound);
    }
    public void PlayStage1Alert()
    {
        MyAudio.PlayOneShot(AlertSound1);
    }
    public void PlayStage2Alert()
    {
        MyAudio.PlayOneShot(AlertSound2);
    }

    public void PlayStage2MapReduce()
    {
        MyAudio.PlayOneShot(MapReduceSound);
    }
    public void PlayStage2PatternC_1(){
        windaudio.clip=windSound;
        windaudio.Play();
    }
    public void pauseStage2PatternC_1(){
        windaudio.Stop();
    }
    // Update is called once per frame
    void Update()
    {

    }
}