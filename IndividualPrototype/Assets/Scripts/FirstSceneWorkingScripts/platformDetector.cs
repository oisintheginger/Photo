using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformDetector : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovementScript>().isGrounded = true;
        }
    }
}
