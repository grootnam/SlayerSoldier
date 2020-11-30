using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPSlider : MonoBehaviour
{
    Slider HPSlider;
    GameObject Enemy;
    float MaxHP;
    float CurrentHP;


    void Start()
    {
        Enemy = GameObject.Find("Enemy");
        CurrentHP = Enemy.GetComponent<EnemyMove>().MonsterHP;
        MaxHP = Enemy.GetComponent<EnemyMove>().MonsterMaxHP;

        HPSlider = GetComponent<Slider>();
        HPSlider.maxValue = MaxHP;
        HPSlider.value = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDoorHP();
    }

    private void UpdateDoorHP()
    {
        //Door의 체력을 계속해서 업데이트
        CurrentHP = Enemy.GetComponent<EnemyMove>().MonsterHP;
        HPSlider.value = CurrentHP;

        //slider의 value가 0이 되도 체력이 조금남은것 처럼 표시되서 아예 꺼버림
        if (CurrentHP <= 0)
        {
            transform.Find("Fill Area").gameObject.SetActive(false);
        }
    }
}
