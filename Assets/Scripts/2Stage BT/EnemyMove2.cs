using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMove2 : MonoBehaviour
{
    float nTime;

    public int phase;
    public float coolTime;

    public float MonsterHP;         // EnemyStatus 스크립트에서 가져옴
    public float MonsterMaxHP;      // EnemyStatus 스크립트에서 가져옴

    public GameObject player;       // 플레이어의 좌표를 받아오기 위해 연동

    public bool patternOn;                 // 패턴이 진행중이라면 true, 아니면 false
    public GameObject warning_B;    // 패턴B의 경고위치를 표기하는 라이트

    public GameObject breath;       // 패턴B의 브레스 이펙트

    void Start()
    {
        nTime = 0;
        coolTime = 7;
        phase = 1;
        patternOn = false;

        MonsterHP = gameObject.GetComponent<EnemyStatus>().HP;
        MonsterMaxHP = gameObject.GetComponent<EnemyStatus>().MaxHP;
    }

    void Update()
    {
        nTime += Time.deltaTime;
        coolTime -= Time.deltaTime;

        MonsterHP = gameObject.GetComponent<EnemyStatus>().HP;
    }



    public bool IsDead()
    {
        if (MonsterHP <= 0)
        {
            return false;
        }
        return true;
    }

    public bool Phase1to2()
    {
        if (phase==1 && MonsterHP <= MonsterMaxHP*0.65f)
        {
            StartCoroutine(ReduceMapSize(phase));

            phase = 2;
            return true;
        }

        return false;
    }
    public bool Phase2to3()
    {
        if (phase == 2 && MonsterHP <= MonsterMaxHP * 0.3f)
        {
            StartCoroutine(ReduceMapSize(phase));

            phase = 3;
            return true;
        }

        return false;
    }

    // 캐릭터를 바라본다.
    public bool MonsterRotation()
    {
        if (!patternOn)
        {
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;

            gameObject.transform.LookAt(target);

            return true;
        }

        // 패턴 진행 중
        else
        {
            return true;
        }

        return false;
    }


    // 쿨타임마다 무작위 패턴 사용
    public bool IsCooltime()
    {
        if (coolTime <= 0)
        {
            coolTime = 4;

            int pattern = Random.Range(1, 3);
            Debug.Log("pattern = " + pattern);

            patternOn = true;

            if (pattern == 1)
            {
                StartCoroutine(PatternA());
            }

            if (pattern == 2)
            {
                StartCoroutine(PatternB());
            }

            if (pattern == 3)
            {
                StartCoroutine(PatternC());
            }

            return true;
        }
        return false;
    }

    IEnumerator PatternA()
    {

        yield return new WaitForSeconds(2f);
        patternOn = false;
    }

    IEnumerator PatternB()
    {
        Vector3 target = player.transform.position;
        target.y = 1;
        warning_B.transform.LookAt(target);
        warning_B.SetActive(true);

        yield return new WaitForSeconds(2f);

        warning_B.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternC()
    {

        yield return new WaitForSeconds(2f);
        patternOn = false;
    }


    //패턴D
    IEnumerator ReduceMapSize(int phase)
    {
        // 스켈레톤 애니메이션 재생
        // 스켈레톤 소리(포효) 재생
        // 맵 줄어드는 소리 재생

        Vector3 north = new Vector3(0, 0, -1);
        Vector3 east = new Vector3(-1, 0, 0);
        Vector3 west = new Vector3(1, 0, 0);
        Vector3 south = new Vector3(0, 0, 1);

        float reduceSize = 1;

        // 100% --> 75%
        if (phase == 1)
        {
            reduceSize = 7.5f;
        }

        // 75% --> 50%
        else if (phase == 2)
        {
            reduceSize = 5;
        }

        for (int i = 0; i < 45; i++)
        {
            yield return new WaitForSeconds(1.5f/45f);

            GameObject.Find("Wall_North").transform.position += north * reduceSize / 45;
            GameObject.Find("Wall_East").transform.position += east * reduceSize / 45;
            GameObject.Find("Wall_West").transform.position += west * reduceSize / 45 ;
            GameObject.Find("Wall_South").transform.position += south * reduceSize / 45;

        }
    }

}