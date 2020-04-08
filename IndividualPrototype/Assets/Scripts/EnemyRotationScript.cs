using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerObject;
    void Awake()
    {
        playerObject = FindObjectOfType<PlayerMovementScript>().gameObject;
        Debug.Log(playerObject.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.forward = playerObject.transform.forward;
        //transform.LookAt(playerObject.transform);
    }
}
