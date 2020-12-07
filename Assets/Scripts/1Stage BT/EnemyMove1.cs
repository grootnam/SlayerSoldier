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
    public GameObject stick;

    public GameObject WarningC; //패턴C의 경고위치를 표기하는 라이트
    public Light Lt;
    public bool ShakeOn;

    Rigidbody m_rigidbody;

    [SerializeField] float m_force = 0f;
    [SerializeField] Vector3 m_offset = Vector3.zero;

    Quaternion m_originRot;
    public GameObject Cam;

    void Start()
    {
        nTime = 0;
        coolTime = 7;
        patternOn = false;

        MonsterHP = gameObject.GetComponent<EnemyStatus>().HP;
        MonsterMaxHP = gameObject.GetComponent<EnemyStatus>().MaxHP;

        m_originRot = Cam.transform.rotation;
    }

    void Update()
    {
        nTime += Time.deltaTime;
        if(!patternOn)
        {
            coolTime -= Time.deltaTime;
        }
        //coolTime -= Time.deltaTime;

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
        if (coolTime <= 0&& !patternOn)
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
        WarningA.SetActive(false);

        GameObject flame = GameObject.Instantiate(prefab_flame) as GameObject;
        flame.transform.position = new Vector3(0, 0, 0);
        flame.transform.LookAt(target);
       
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
            warningpos[i] = WarningB.transform.forward;
        }
        yield return new WaitForSeconds(2f);
        //WarningB.SetActive(false);


        for (int i=0;i<8;i++)
        {
            GameObject MultiBall = GameObject.Instantiate(BigBall) as GameObject;
            m_rigidbody = MultiBall.GetComponent<Rigidbody>();
            MultiBall.transform.position = new Vector3(0, 3, 0);
            MultiBall.transform.LookAt(warningpos[i]);
            m_rigidbody.AddForce(warningpos[i] * 50f);

        }
        WarningB.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternC()
    {
        WarningC.SetActive(true);
        WarningC.GetComponent<Light>().range = 50f;
        while (WarningC.GetComponent<Light>().range >= 20f && ShakeOn == false)
        {
            WarningC.GetComponent<Light>().range -= 0.5f;
            yield return new WaitForSeconds(0.05f);
        }
        WarningC.SetActive(false);
        //Shake();
        if (WarningC.GetComponent<Light>().range<25f)
        {
            StartCoroutine(Cameramove());
            
            yield return new WaitForSeconds(1f);
            StartCoroutine(CameramoveReset());
        }

        //if (!GetComponent<PlayerMove>().isShiftskill && !GetComponent<PlayerMove>().canJump)
        //    {
        //        GetComponent<PlayerMove>().PlayerHP -= 70;
        //    }
        patternOn = false;
    }

    //void Shake()
    //{
    //    StartCoroutine(Cameramove());
    //}
    IEnumerator Cameramove()
    {
        ShakeOn = true;
        Vector3 t_originEuler = Cam.transform.eulerAngles;
        while (true)
        {
            float t_rotX = Random.Range(-m_offset.x, m_offset.x);
            float t_rotY = Random.Range(-m_offset.y, m_offset.y);
            float t_rotZ = Random.Range(-m_offset.z, m_offset.z);
            Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
            Quaternion t_rot = Quaternion.Euler(t_randomRot);
            while (Quaternion.Angle(transform.rotation, t_rot) > 0.1f)
            {
                Cam.transform.rotation = Quaternion.RotateTowards(Cam.transform.rotation, t_rot, m_force * Time.deltaTime);
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator CameramoveReset()
    {
        ShakeOn = false;
        while (Quaternion.Angle(Cam.transform.rotation, m_originRot) > 0f)
        {
            Cam.transform.rotation = Quaternion.RotateTowards(Cam.transform.rotation, m_originRot, m_force * Time.deltaTime);
            yield return null;
        }
    }
}