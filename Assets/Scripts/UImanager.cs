using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using JetBrains.Annotations;
using System.IO;

public class UImanager : MonoBehaviour
{
    public bool isStageOver; // 스테이지 종료 여부

    public Text Bullet;
    public Text Qskill;
    public Text Eskill;

    public Text Shiftskill;
    public Text Shiftskillcool;
    GameObject Player;

    public GameObject ResultPopupClear;
    public GameObject ResultPopupOver;

    bool IsPause;

    void Start()
    {
        ResultPopupClear.SetActive(false);
        ResultPopupOver.SetActive(false);
        IsPause = false;
    }

    void Update()
    {
        //"T" -> 일시정지
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(IsPause==false)
            {
                Time.timeScale = 0;
                IsPause = true;
                return;
            }
            if(IsPause==true)
            {
                Time.timeScale = 1;
                IsPause = false;
                return;
            }
        }
        //bullet개수
        int leftBullet = GameObject.Find("Player").GetComponent<PlayerMove>().leftBulletNum;
        Bullet.text = leftBullet.ToString();
        
        //스킬 쿨타임 표시
        float shiftcool=GameObject.Find("Player").GetComponent<PlayerMove>().Shift_temptime;
        Shiftskillcool.text="("+shiftcool.ToString("N1")+")";


        //Q, E, Shift 스킬
        if (Input.GetKey(KeyCode.Q))
        {
            Qskill.text = "<color=#ff0000>" + "Q" + "</color>";
        }
        else
        {
            Qskill.text = "<color=#ffffff>" + "Q" + "</color>";
        }

        if (Input.GetKey(KeyCode.E))
        {
            Eskill.text = "<color=#ff0000>" + "E" + "</color>";
        }
        else
        {
            Eskill.text = "<color=#ffffff>" + "E" + "</color>";
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Shiftskill.text = "<color=#ff0000>" + "Shift" + "</color>";
        }
        else
        {
            Shiftskill.text = "<color=#ffffff>" + "Shift" + "</color>";
        }
    }

    public void stageClear()
    {
        isStageOver = true;
        Time.timeScale = 0;
        ResultPopupClear.SetActive(true);
    }
    public void stageOver()
    {
        isStageOver = true;
        Time.timeScale = 0;
        ResultPopupOver.SetActive(true);
    }
}
