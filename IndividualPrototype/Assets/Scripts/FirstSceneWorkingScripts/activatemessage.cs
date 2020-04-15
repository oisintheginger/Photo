using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class activatemessage : MonoBehaviour
{
    [SerializeField] GameObject messagetoEnable;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {

            messagetoEnable.SetActive(true);
        }
    }
}
