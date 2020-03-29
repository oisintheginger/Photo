using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShoot : MonoBehaviour
{
    [SerializeField]Camera heldCamera;
    Ray camerashootRay;
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
            Shoot();
        }
    }

    void Shoot()
    {
        camerashootRay = new Ray(heldCamera.transform.position, heldCamera.transform.forward);
        RaycastHit rh;
        if (Physics.Raycast(camerashootRay, out rh, 50f))
        {
            if (rh.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("Triggerd");
            }
        }
    }

}
