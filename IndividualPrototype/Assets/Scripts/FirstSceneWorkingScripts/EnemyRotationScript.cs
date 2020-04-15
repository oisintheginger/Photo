using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationScript : MonoBehaviour
{
    GameObject playerObject;
    void Awake()
    {
        playerObject = FindObjectOfType<PlayerMovementScript>().gameObject;
    }

    void FixedUpdate()
    {
        transform.LookAt(new Vector3(playerObject.transform.position.x, this.transform.position.y, playerObject.transform.position.z));
    }
}
