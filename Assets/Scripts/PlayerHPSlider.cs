using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPSlider : MonoBehaviour
{
    Slider HPSlider;
    GameObject Player;
    float MaxHP;
    float CurrentHP;


    void Start()
    {
        Player = GameObject.Find("Player");
        CurrentHP = Player.GetComponent<PlayerMove>().PlayerHP;
        //MaxHP = Player.GetComponent<PlayerMove>().MaxHP;

        HPSlider = GetComponent<Slider>();
        HPSlider.maxValue = CurrentHP;
        HPSlider.value = CurrentHP;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDoorHP();
    }

    private void UpdateDoorHP()
    {
        //Door의 체력을 계속해서 업데이트
        CurrentHP = Player.GetComponent<PlayerMove>().PlayerHP;
        HPSlider.value = CurrentHP;

        //slider의 value가 0이 되도 체력이 조금남은것 처럼 표시되서 아예 꺼버림
        if (CurrentHP <= 0)
        {
            transform.Find("Fill Area").gameObject.SetActive(false);
        }
    }
}
