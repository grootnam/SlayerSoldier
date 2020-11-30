using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    GameObject Enemy;
    GameObject Player;
    void Start()
    {
        Enemy = GameObject.Find("Enemy");
        Player = GameObject.Find("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Enemy.GetComponent<EnemyStatus>().HP -= Player.GetComponent<PlayerMove>().PlayerAttackPower * 100;

            Debug.Log("missile & enemy 충돌");

            // 충돌 사운드

            // 충돌 효과

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "map")
        {
            Debug.Log("missile & map 충돌");
            Destroy(gameObject);
        }
    }
}