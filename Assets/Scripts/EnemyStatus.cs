using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float MaxHP;
    public float HP;


    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0f && !FindObjectOfType<UImanager>().isStageOver)
        {
            FindObjectOfType<UImanager>().stageClear();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
