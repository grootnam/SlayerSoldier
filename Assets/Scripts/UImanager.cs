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

    GameObject Player;

    public GameObject ResultPopup;

    bool IsPause;

    void Start()
    {
        ResultPopup.SetActive(false);
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
        
        //Q, E 스킬
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
    }

    public void stageClear()
    {
        isStageOver = true;
        ResultPopup.SetActive(true);
    }
}
