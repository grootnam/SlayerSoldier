using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    float nTime;

    /* monster ability
     */
    public float MonsterMaxHP = 500;
    public float MonsterHP;
    public int phase;
    public float coolTime;


    void Start()
    {
        nTime = 0;


        MonsterHP = MonsterMaxHP;
        coolTime = 4;
        phase = 1;
    }

    void Update()
    {
        nTime += Time.deltaTime;
        coolTime -= Time.deltaTime;
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
            phase = 2;

            /*
             * ���� �� ũ�� 100%-->75%�� ���̴� �ڵ�
             */

            return true;
        }

        return false;
    }
    public bool Phase2to3()
    {
        if (phase == 2 && MonsterHP <= MonsterMaxHP * 0.3f)
        {
            phase = 3;

            /*
             * ���� �� ũ�� 75%-->50%�� ���̴� �ڵ�
             */
            return true;
        }

        return false;
    }


    public bool IsCooltime()
    {
        if (coolTime <= 0)
        {
            coolTime = 4;

            int pattern = Random.Range(1, 3);

            if (pattern == 1)
            {
                //����A
            }

            if (pattern == 2)
            {
                //����B
            }

            if (pattern == 3)
            {
                //����C
            }

            return true;
        }
        return false;
    }

    public bool MonsterRotation()
    {
        if (MonsterHP > 0)
        {
            /*
             * ���� ���Ͱ� �÷��̾� �ٶ󺸰� ȸ���ϴ� ����
             */

            return true;
        }
        return false;
    }

}