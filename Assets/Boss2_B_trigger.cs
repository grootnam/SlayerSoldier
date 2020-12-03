using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_B_trigger : MonoBehaviour
{
    BoxCollider trigger;
    float move;
    float speed;


    private void Start()
    {
        trigger = gameObject.GetComponent<BoxCollider>();
        trigger.center = new Vector3(0, 0, 0);
        trigger.size = new Vector3(0, 0, 0.1f);
        move = 0f;
        speed = 10.0f;
    }

    private void Update()
    {
        move += Time.deltaTime * speed;
        trigger.center = new Vector3(0, 0, move);
        trigger.size = new Vector3(move, move, 0.1f);
    }





}
