using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transportScript : MonoBehaviour
{
    [SerializeField] Transform teleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            other.gameObject.transform.position = teleportPoint.position;
            other.gameObject.transform.rotation = teleportPoint.rotation;
        }
    }
}
