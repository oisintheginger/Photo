using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Transform endpoint;
    [SerializeField] float speed;
    public void MoveToPosition()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, endpoint.position, speed*Time.deltaTime);
    }
}
