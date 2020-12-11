using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI2 : MonoBehaviour
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence seqMovingAttack = new Sequence();
    private Sequence seqDead = new Sequence();


    private IsDead m_IsDead = new IsDead();

    private Phase1to2 m_Phase1to2 = new Phase1to2();
    private Phase2to3 m_Phase2to3 = new Phase2to3();
    private IsCooltime m_IsCooltime = new IsCooltime();
    private MonsterRotation m_MonsterRotation = new MonsterRotation();


    private EnemyMove2 m_Enemy;
    private IEnumerator behaviorProcess;


    void Start()
    {
        Debug.Log("Start Tree");
        m_Enemy = gameObject.GetComponent<EnemyMove2>();
        root.AddChild(selector);
        selector.AddChild(seqDead);
        selector.AddChild(seqMovingAttack);


        // node화
        m_IsDead.Enemy = m_Enemy;

        m_Phase1to2.Enemy = m_Enemy;
        m_Phase2to3.Enemy = m_Enemy;
        m_IsCooltime.Enemy = m_Enemy;
        m_MonsterRotation.Enemy = m_Enemy;


        // 자식노드 연결
        seqDead.AddChild(m_IsDead);

        seqMovingAttack.AddChild(m_Phase1to2);
        seqMovingAttack.AddChild(m_Phase2to3);
        seqMovingAttack.AddChild(m_IsCooltime);
        seqMovingAttack.AddChild(m_MonsterRotation);
        Debug.Log("Stage2Node");

        behaviorProcess = BehaviorProcess();
        StartCoroutine(behaviorProcess);
    }

    public IEnumerator BehaviorProcess()
    {
        while (root.Invoke())
        {
            yield return new WaitForEndOfFrame();
        }
        //Destroy(gameObject, 0.0f);
        Debug.Log("behavior process exit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
