using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMove2 : MonoBehaviour
{
    public int phase;           // monster has 3 phases. Map will be reduced when phase is changed.
    public float coolTime;      // pattern cool-time. 

    float MonsterHP;         // Get values from <EnemyStatus> script
    float MonsterMaxHP;      // Get values from <EnemyStatus> script
    GameObject player;

    public bool patternOn;                 // make TRUE when using pattern.

    public int Pa_A_SpearPower = 50;        // for pattern A, speed at which a spear is blown
    public GameObject prefab_A_Object;      // for pattern A, skill effect

    public GameObject B_warning;            // for pattern B, warning light
    public GameObject prefab_B_flame;       //  for pattern B, skill effect

    public GameObject PatternC_range;       // for pattern C, warning effect

    void Awake()
    {
        player = GameObject.Find("Player");
        MonsterHP = gameObject.GetComponent<EnemyStatus>().HP;
        MonsterMaxHP = gameObject.GetComponent<EnemyStatus>().MaxHP;
    }

    private void Start()
    {
        phase = 1;
        coolTime = 7;   // Applies only at start. (for wake-up directing)
        patternOn = false;
    }

    void Update()
    {
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

    /*
     * Pattern D : change phase and reduce map size.
     */
    public bool Phase1to2()
    {
        if (phase==1 && (MonsterHP <= (MonsterMaxHP*0.65f)))
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
            Debug.Log("?????? ????????????");
            StartCoroutine(ReduceMapSize(phase));

            phase = 3;
            return true;
        }

        return false;
    }


    // monster lotate to look the player.
    public bool MonsterRotation()
    {
        if (!patternOn)
        {
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;

            gameObject.transform.LookAt(target);

            return true;
        }

        // when using pattern, do not lotate.
        else if (patternOn)
        {
            return true;
        }

        return false;
    }


    // Play pattern 
    public bool IsCooltime()
    {
        if (coolTime <= 0 && patternOn==false)
        {
            coolTime = 4;

            int pattern = Random.Range(1, 4); //(1-3)?????? ?????? ???

            Debug.Log("pattern = " + pattern);

            // ?????? ?????? ???
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
        // ???????????? ?????? (3-5 ?????? ??????)
        int count=Random.Range(3, 6);
        List<GameObject> Spearlist = new List<GameObject>();

        for(int i=1;i<=count;i++)
        {
            // Boss looking throw target
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(target);

            // generate spear at back of the boss
            GameObject Spear = Instantiate(prefab_A_Object) as GameObject;
            Spear.transform.parent = gameObject.transform;
            if(i%2==0)
                Spear.transform.localPosition=new Vector3(0.3f*-(Mathf.CeilToInt((float)i/2f)),1,0);
            else
                Spear.transform.localPosition=new Vector3(0.3f*Mathf.CeilToInt((float)i/2f),1,0);
            
            Spearlist.Add(Spear);
            SoundManager.Instance.Stage2PatternAsword();
            yield return new WaitForSeconds(0.15f);
        }

        for(int i=0;i<count;i++)
        {
            // Boss see a throwing-object
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(target);

            // make spear orient the player
            Spearlist[i].transform.parent=null;
            Spearlist[i].transform.LookAt(player.transform.position);
            Spearlist[i].transform.eulerAngles = Spearlist[i].transform.eulerAngles+new Vector3(90,0,0);

            // set direction for throw
            Vector3 dir = player.transform.position - Spearlist[i].transform.position;
            dir.Normalize();

            // throw
            Spearlist[i].GetComponent<Rigidbody>().AddForce(dir * Pa_A_SpearPower, ForceMode.Impulse);

            // play Spear-Throw sound
            SoundManager.Instance.Stage2PatternAthrowing();

            yield return new WaitForSeconds(0.5f);
        }
        //3?????? ??? ????????????
        StartCoroutine(DeleteSpear(1.5f,Spearlist));
        patternOn = false;
    }
    
    IEnumerator DeleteSpear(float second,List<GameObject> Spearlist)
    {
        yield return new WaitForSeconds(second);
        for(int i=0;i<Spearlist.Count;i++){
            Destroy(Spearlist[i]);
        }
    }


    IEnumerator PatternB()
    {
        // ?????? ?????? ??????
        Vector3 target = player.transform.position;
        target.y = 1;

        // ?????? ????????? ?????????
        B_warning.transform.LookAt(target);

        // ????????? ?????????
        B_warning.SetActive(true);

        // ????????? ??????
        SoundManager.Instance.PlayStage2Alert();

        // 2??? ???
        yield return new WaitForSeconds(2f);

        // ?????? ??????
        GameObject B_flame = GameObject.Instantiate(prefab_B_flame) as GameObject; 
        B_flame.transform.position = new Vector3(0, 0, 0);
        B_flame.transform.LookAt(target);
        
        // ?????? ?????????
        SoundManager.Instance.Stage2PatternB();

        // ????????? ????????????
        B_warning.SetActive(false);
        patternOn = false;
    }

    IEnumerator PatternC()
    {
        gameObject.GetComponent<Animator>().SetBool("Ccast",true);
        SoundManager.Instance.PlayStage2PatternC_1();
        for(int i=0;i<500;i++)
        {
            //????????? ???????????? ?????????
            Vector3 target = player.transform.position;
            target.y = gameObject.transform.position.y;
            gameObject.transform.LookAt(target);

            //???????????????
            Vector3 bosspos=gameObject.transform.position;
            bosspos.y=player.transform.position.y;
            Vector3 dir=bosspos-player.transform.position;
            dir.Normalize();
            player.transform.Translate(dir*0.15f,Space.World);
            yield return new WaitForSeconds(0.01f);
        }
        SoundManager.Instance.pauseStage2PatternC_1();

        //??????????????? ??????
        SoundManager.Instance.Stage2PatternC();
        gameObject.GetComponent<Animator>().SetBool("PatternC",true);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Animator>().SetBool("Ccast",false);
        gameObject.GetComponent<Animator>().SetBool("PatternC",false);

        //?????????
        PatternC_range.SetActive(true);
        yield return new WaitForSeconds(1f);
        PatternC_range.SetActive(false);
        yield return new WaitForSeconds(2f);
        patternOn = false;
    }


    // pattern D 
    IEnumerator ReduceMapSize(int phase)
    {
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

        // moves wall slowly 
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