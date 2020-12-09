using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("map")||other.gameObject.layer==LayerMask.NameToLayer("ground"))
        {
            gameObject.GetComponent<Rigidbody>().velocity=Vector3.zero;
            Destroy(gameObject,2f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer==LayerMask.NameToLayer("map")||other.gameObject.layer==LayerMask.NameToLayer("ground"))
        {
            gameObject.GetComponent<Rigidbody>().velocity=Vector3.zero;
            Destroy(gameObject,2f);
        }
    }
}
