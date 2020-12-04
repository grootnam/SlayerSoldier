using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove1 : MonoBehaviour
{
    float nTime;

    public int phase;
    public float coolTime;

    public float MonsterHP;         // EnemyStatus 스크립트에서 가져옴
    public float MonsterMaxHP;      // EnemyStatus 스크립트에서 가져옴

    public GameObject player;       // 플레이어의 좌표를 받아오기 위해 연동

    public bool patternOn;                 // 패턴이 진행중이라면 true, 아니면 false


    public GameObject WarningA;    // 패턴A의 경고위치를 표기하는 라이트
    public GameObject prefab_flame;      // 패턴A 이펙트 및 트리거
    public GameObject BigBall; //패턴B 구체프리팹
    public GameObject WarningB; // 패턴B의 경고위치를 표기하는 막대

    Rigidbody m_rigidbody;

    void Start()
    {
        nTime = 0;
        coolTime = 7;
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

    
    // 캐릭터 바라봄
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
        else if (patternOn)
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
        Vector3 target = player.transform.position;
        WarningA.SetActive(true);
        yield return new WaitForSeconds(1f);

        GameObject flame = GameObject.Instantiate(prefab_flame) as GameObject;
        flame.transform.position = new Vector3(0, 0, 0);
        flame.transform.LookAt(target);

        WarningA.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternB()
    {
        WarningB.SetActive(true);
        Vector3[] warningpos = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            GameObject MultiWarning = GameObject.Instantiate(WarningB) as GameObject;
            MultiWarning.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360),0);
            warningpos[i] = MultiWarning.transform.forward;
            warningpos[i].y = 0;
            //Debug.Log(warningpos[i]);
        }
        yield return new WaitForSeconds(3f);

        for(int i=0;i<8;i++)
        {
            GameObject MultiBall = GameObject.Instantiate(BigBall) as GameObject;
            m_rigidbody = MultiBall.GetComponent<Rigidbody>();
            MultiBall.transform.position = new Vector3(0, 0, 0);
            MultiBall.transform.LookAt(warningpos[i]);
            m_rigidbody.AddForce(warningpos[i] * 100f);
        }

        
        WarningB.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternC()
    {

        yield return new WaitForSeconds(2f);
        patternOn = false;
    }

}