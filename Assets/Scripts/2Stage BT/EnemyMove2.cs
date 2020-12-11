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
    public int Pa_A_SpearPower=50;
    public GameObject player;       // �÷��̾��� ��ǥ�� �޾ƿ��� ���� ����

    public bool patternOn;                 // ������ �������̶�� true, �ƴϸ� false


    public GameObject B_warning;    // ����B�� �����ġ�� ǥ���ϴ� ����Ʈ
    public GameObject prefab_B_flame;      // ����B�� ����Ʈ �� Ʈ����
    public GameObject prefab_A_Object;
    public GameObject PatternC_range;
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
            gameObject.GetComponent<Animator>().SetBool("Dead", true);
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
        else if (patternOn)
        {
            return true;
        }

        return false;
    }


    // ��Ÿ�Ӹ��� ������ ���� ���
    public bool IsCooltime()
    {
        if (coolTime <= 0&&patternOn==false)
        {
            coolTime = 4;

            int pattern = Random.Range(1, 4); //(1-3)범위 랜덤 수
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
        //투사체의 개수 (3-5 사이 랜덤)
        int count=Random.Range(3, 6);
        List<GameObject> Spearlist = new List<GameObject>();
        for(int i=1;i<=count;i++)
        {
            //투사체 쳐다보고있기
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(target);

            //창 생성.
            GameObject Spear=Instantiate(prefab_A_Object);
            Spear.transform.parent=gameObject.transform;
            if(i%2==0)
                Spear.transform.localPosition=new Vector3(0.3f*-(Mathf.CeilToInt((float)i/2f)),1,0);
            else
                Spear.transform.localPosition=new Vector3(0.3f*Mathf.CeilToInt((float)i/2f),1,0);
            
            Spearlist.Add(Spear);
            SoundManager.Instance.Stage2PatternAsword();
            yield return new WaitForSeconds(1f);
        }

        for(int i=0;i<count;i++)
        {
            //투사체 쳐다보고있기
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(target);

            //창 내려주고 발사
            Spearlist[i].transform.LookAt(player.transform.position);
            Vector3 dir=player.transform.position-Spearlist[i].transform.position;
            dir.Normalize();
            Spearlist[i].transform.eulerAngles=Spearlist[i].transform.eulerAngles+new Vector3(90,0,0);
            Spearlist[i].GetComponent<Rigidbody>().AddForce(dir*Pa_A_SpearPower,ForceMode.Impulse);
            SoundManager.Instance.Stage2PatternAthrowing();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(5f);
        patternOn = false;
    }

    IEnumerator PatternB()
    {
        Vector3 target = player.transform.position;
        target.y = 1;
        B_warning.transform.LookAt(target);
        B_warning.SetActive(true);
        SoundManager.Instance.PlayStage2Alert();

        yield return new WaitForSeconds(2f);

        GameObject B_flame = GameObject.Instantiate(prefab_B_flame) as GameObject; 
        B_flame.transform.position = new Vector3(0, 0, 0);
        B_flame.transform.LookAt(target);
        SoundManager.Instance.Stage2PatternB();

        B_warning.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternC()
    {
        for(int i=0;i<70;i++)
        {
            Vector3 dir=gameObject.transform.position-player.transform.position;
            dir.y=0;
            dir.Normalize();
            player.GetComponent<Rigidbody>().AddForce(dir*1.5f,ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        //애니메이션 실행
        SoundManager.Instance.Stage2PatternC();
        gameObject.GetComponent<Animator>().SetBool("PatternC",true);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Animator>().SetBool("PatternC",false);

        //데미지
        PatternC_range.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        PatternC_range.SetActive(false);
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
        SoundManager.Instance.PlayStage2MapReduce();
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