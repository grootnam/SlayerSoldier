using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMove2 : MonoBehaviour
{
    float nTime;

    public int phase;
    public float coolTime;

    public float MonsterHP;         // EnemyStatus ��ũ��Ʈ���� ������
    public float MonsterMaxHP;      // EnemyStatus ��ũ��Ʈ���� ������

    public GameObject player;       // �÷��̾��� ��ǥ�� �޾ƿ��� ���� ����

    public bool patternOn;                 // ������ �������̶�� true, �ƴϸ� false
    public GameObject warning_B;    // ����B�� �����ġ�� ǥ���ϴ� ����Ʈ

    public GameObject breath;       // ����B�� �극�� ����Ʈ

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

    // ĳ���͸� �ٶ󺻴�.
    public bool MonsterRotation()
    {
        if (!patternOn)
        {
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;

            gameObject.transform.LookAt(target);

            return true;
        }

        // ���� ���� ��
        else
        {
            return true;
        }

        return false;
    }


    // ��Ÿ�Ӹ��� ������ ���� ���
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


    //����D
    IEnumerator ReduceMapSize(int phase)
    {
        // ���̷��� �ִϸ��̼� ���
        // ���̷��� �Ҹ�(��ȿ) ���
        // �� �پ��� �Ҹ� ���

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