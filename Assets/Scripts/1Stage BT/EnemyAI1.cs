using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI1 : MonoBehaviour
{
    private Sequence1 root1 = new Sequence1();
    private Selector1 selector1 = new Selector1();
    private Sequence1 seqMovingAttack1 = new Sequence1();
    private Sequence1 seqDead1 = new Sequence1();


    private IsDead1 m_IsDead1 = new IsDead1();

    private phase1 m_phase1 = new phase1();
    private phase2 m_phase2 = new phase2();
    private phase3 m_phase3 = new phase3();
    private MonsterRotation1 m_MonsterRotation1 = new MonsterRotation1();


    private EnemyMove1 m_Enemy1;
    private IEnumerator behaviorProcess1;


    void Start()
    {
        Debug.Log("Start Tree");
        m_Enemy1 = gameObject.GetComponent<EnemyMove1>();
        root1.AddChild(selector1);
        selector1.AddChild(seqDead1);
        selector1.AddChild(seqMovingAttack1);


        // node화
        m_IsDead1.Enemy = m_Enemy1;

        m_phase1.Enemy = m_Enemy1;
        m_phase2.Enemy = m_Enemy1;
        m_phase3.Enemy = m_Enemy1;
        m_MonsterRotation1.Enemy = m_Enemy1;


        // 자식노드 연결
        

        seqMovingAttack1.AddChild(m_phase1);
        seqMovingAttack1.AddChild(m_phase2);
        seqMovingAttack1.AddChild(m_phase3);
        seqMovingAttack1.AddChild(m_MonsterRotation1);
        seqDead1.AddChild(m_IsDead1);

        behaviorProcess1 = BehaviorProcess1();
        StartCoroutine(behaviorProcess1);
    }

    public IEnumerator BehaviorProcess1()
    {
        while (root1.Invoke())
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
