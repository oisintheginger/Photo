using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    public AudioSource PlayerAudioSource;
    [SerializeField] AudioClip footstepSound;
    public float maxHealth, health;

    [SerializeField] string horizontalAxis, verticalAxis, turningAxis, jumpAxis;
    [SerializeField] Image healthBar;

    Rigidbody pRB;
    [SerializeField] Vector3 appliedForce;
    public float maxSpeed, minSpeed, maxForce, boostForce, accelerationForce, brakeForce, xZPlaneSpeed, jumpForce, turnSpeed, turnScaler;
    float storedMaxSpeed;
    [SerializeField, Range(0, 1)] float steeringScaler;
    public bool isGrounded;
    [SerializeField] Transform groundTransform, slopeTransform;


    Ray groundCheckRay;
    private void Awake()
    {
        pRB = this.gameObject.GetComponent<Rigidbody>();
       // pRB.drag = 0f;
        storedMaxSpeed = maxSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        groundCheckRay = new Ray(groundTransform.position, -transform.up);
        Motion();
        healthBar.fillAmount = health / maxHealth;
        FootStepSound();
        
    }


    float timer = 0.5f;
    public void FootStepSound()
    {
        if(Mathf.Abs(Input.GetAxis("Vertical"))>0&&isGrounded|| Mathf.Abs(Input.GetAxis("Horizontal")) > 0&&isGrounded)
        {
            if (timer <= 0f)
            {
                PlayerAudioSource.PlayOneShot(footstepSound);
                timer = 0.6f;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        
    }
    

    void GroundCheck()
    {
        RaycastHit rH;
        if (Physics.Raycast(groundTransform.position, -transform.up, out rH, 0.5f))
        {
            if (rH.collider.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
    }


    void Motion()
    {

        var velocity = pRB.velocity;

        xZPlaneSpeed = Mathf.Sqrt((velocity.x * velocity.x) + (velocity.z * velocity.z));

        Vector3 normalizedXZMovement = new Vector3(velocity.x / xZPlaneSpeed, velocity.y, velocity.z / xZPlaneSpeed);

        Vector3 xzSpeed = new Vector3(normalizedXZMovement.x * maxSpeed, velocity.y, normalizedXZMovement.z * maxSpeed);

        if  (!isGrounded)
        {
            maxSpeed = minSpeed;
        }
        else
        {
            maxSpeed = storedMaxSpeed;
        }
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5)
        {
            pRB.AddForce((transform.forward) * accelerationForce * Input.GetAxisRaw("Vertical"), ForceMode.VelocityChange);
            pRB.AddForce((transform.right) * accelerationForce * Input.GetAxisRaw("Horizontal"), ForceMode.VelocityChange);

            if (xZPlaneSpeed >= maxSpeed)
            {
                pRB.velocity = xzSpeed;
            }
        }
        

        if (Input.GetAxis(jumpAxis) > 0.9f && isGrounded)
        {
            pRB.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }

    }

}
