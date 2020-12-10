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

    public GameObject WarningC; //패턴C의 경고위치를 표기하는 라이트
    public Light Lt;
    
    [SerializeField] float m_force = 0f;
    [SerializeField] Vector3 m_offset = Vector3.zero;

    public GameObject cam;
    Coroutine shake;

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
        if(!patternOn)
        {
            coolTime -= Time.deltaTime;
        }

        MonsterHP = gameObject.GetComponent<EnemyStatus>().HP;
    }

    public bool IsDead()
    {
        if (MonsterHP <= 0)
        {
            //gameObject.GetComponent<Animator>().SetBool("Dead", true);
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
        if (coolTime <= 0 && !patternOn)
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
        SoundManager.Instance.Stage1PatternA();
        yield return new WaitForSeconds(2f);
    }

    IEnumerator PatternB()
    {
        WarningB.SetActive(true);
        Vector3[] warningpos = new Vector3[8];

        System.Array.Clear(warningpos, 0, warningpos.Length);

        for (int i = 0; i < 8; i++)
        {
            GameObject MultiWarning = GameObject.Instantiate(WarningB) as GameObject;
            MultiWarning.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360),0);
            warningpos[i] = MultiWarning.transform.forward;
        }
        yield return new WaitForSeconds(2f);
        //WarningB.SetActive(false);


        for (int i = 0; i < 8; i++)
        {
            GameObject MultiBall = GameObject.Instantiate(BigBall);
            //m_rigidbody = MultiBall.GetComponent<Rigidbody>();
            MultiBall.transform.position = new Vector3(warningpos[i].x*2f, 1.5f, warningpos[i].z*2f);
            MultiBall.transform.LookAt(warningpos[i]);
            MultiBall.GetComponent<Rigidbody>().AddForce(warningpos[i] * 1000f);
            //MultiBall.transform.Translate(warningpos[i] * Time.deltaTime);
            //m_rigidbody.AddForce(warningpos[i] * 50f);
        }
        yield return new WaitForSeconds(2f);
        WarningB.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternC()
    {
        WarningC.SetActive(true);
        WarningC.GetComponent<Light>().range = 50f;
        while (WarningC.GetComponent<Light>().range >= 20f)
        {
            WarningC.GetComponent<Light>().range -= 0.5f;
            yield return new WaitForSeconds(0.03f);
        }
        WarningC.SetActive(false);
        shake = StartCoroutine(Shake());
        SoundManager.Instance.Stage1PatternC();
        Debug.Log("흔들기 시작");
        yield return new WaitForSeconds(0.5f);
        if (!player.GetComponent<PlayerMove>().isNoDamagetime && player.GetComponent<PlayerMove>().canJump)
        {
            player.GetComponent<PlayerMove>().PlayerHP -= 70;
        }
        StopCoroutine(shake);
        Debug.Log("흔들기 끝");

        yield return new WaitForSeconds(2f);
        
        patternOn = false;
    }

    IEnumerator Shake()
    {
        Vector3 t_originEuler = cam.transform.eulerAngles;
        while (true)
        {
            float t_rotX = Random.Range(-m_offset.x, m_offset.x);
            float t_rotY = Random.Range(-m_offset.y, m_offset.y);
            float t_rotZ = Random.Range(-m_offset.z, m_offset.z);
            Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
            Quaternion t_rot = Quaternion.Euler(t_randomRot);
            while (Quaternion.Angle(cam.transform.rotation, t_rot) > 0.1f)
            {
                cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, t_rot, m_force * Time.deltaTime);
                yield return null;
            }

            yield return null;
        }
    }
}