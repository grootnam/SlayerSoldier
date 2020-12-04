using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI1 : MonoBehaviour
{
    private Sequence1 root = new Sequence1();
    private Selector1 selector = new Selector1();
    private Sequence1 seqMovingAttack = new Sequence1();
    private Sequence1 seqDead = new Sequence1();


    private IsDead1 m_IsDead = new IsDead1();

    private IsCooltime1 m_IsCooltime = new IsCooltime1();
    private MonsterRotation1 m_MonsterRotation = new MonsterRotation1();


    private EnemyMove1 m_Enemy;
    private IEnumerator behaviorProcess;


    void Start()
    {
        Debug.Log("Start Tree");
        m_Enemy = gameObject.GetComponent<EnemyMove1>();
        root.AddChild(selector);
        selector.AddChild(seqDead);
        selector.AddChild(seqMovingAttack);


        // node화
        m_IsDead.Enemy = m_Enemy;


        m_IsCooltime.Enemy = m_Enemy;
        m_MonsterRotation.Enemy = m_Enemy;


        // 자식노드 연결
        seqDead.AddChild(m_IsDead);

        seqMovingAttack.AddChild(m_IsCooltime);
        seqMovingAttack.AddChild(m_MonsterRotation);


        behaviorProcess = BehaviorProcess();
        StartCoroutine(behaviorProcess);
    }

    public IEnumerator BehaviorProcess()
    {
        while (root.Invoke())
        {
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("behavior process exit");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
