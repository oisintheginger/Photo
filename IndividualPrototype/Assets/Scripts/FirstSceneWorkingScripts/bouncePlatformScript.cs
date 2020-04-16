using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bouncePlatformScript : MonoBehaviour
{

    [SerializeField]TMP_Text thresholdIndicator;
    public TMP_Text activeText;

    public bool isBouncy =false;
    public float bounceThreshhold, underBounce;
    public float bounceForce;
    public float maxBounce;

    private void Awake()
    {
        if (thresholdIndicator != null && activeText != null)
        {
            thresholdIndicator.text = "Velocity threshold: " + bounceThreshhold.ToString() + "M/s";
            if (isBouncy)
            {
                activeText.text = "Active";
            }
            else
            {
                activeText.text = "Not Active";
            }
        }
    }

    //Vector3 debugVector1, debugVector2;

    //Vector3 incomingVelocity;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player"&&isBouncy==true)
        {
            
            var tobounce = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 newVelocity = new Vector3(tobounce.velocity.x, -collision.relativeVelocity.y + 2f, tobounce.velocity.z) + (collision.relativeVelocity.y * bounceForce * collision.GetContact(0).normal);
            if (Mathf.Abs(collision.relativeVelocity.y) < bounceThreshhold)
            {
                newVelocity = Vector3.ClampMagnitude(newVelocity, underBounce);

            }
            else
            {
                
                newVelocity = Vector3.ClampMagnitude(newVelocity, maxBounce);
            }
            tobounce.velocity = newVelocity;


        }
    }

    

    /*
    private void OnDrawGizmos()
    {
        if(debugVector2!=null)
        {
            Gizmos.DrawLine(debugVector1, debugVector2);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            var tobounce = other.gameObject.GetComponent<Rigidbody>();
            incomingVelocity = tobounce.velocity;
        }
    }
    */
    public void ActivateBounce()
    {
        isBouncy = true;
    }
}
