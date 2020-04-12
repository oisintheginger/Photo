using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotionScript : MonoBehaviour
{
    Vector2 mouseL;
    Vector2 smoothV;
    public float sensitivityX,minX,maxX, sensitivityY,minY,maxY;
    public float smoothing;

    float playerVelocity, playerMaxVelocity;

    float timer = 0f;
    [SerializeField]float bobSpeed = .2f, maxBobSpeed;
    [SerializeField] float bobAmount = 0.2f;
    float midpoint = 0.5f;


    GameObject character;

    private void Start()
    {
        maxX = sensitivityX;
        maxY = sensitivityY;
        character = this.transform.parent.gameObject;
        
    }


    // First person camera script Found here: https://www.youtube.com/watch?v=blO039OzUZc
    private void FixedUpdate()
    {
        
        playerVelocity = this.transform.parent.gameObject.GetComponent<PlayerMovementScript>().xZPlaneSpeed;
        playerMaxVelocity = this.transform.parent.gameObject.GetComponent<PlayerMovementScript>().maxSpeed;

        float tempX = Mathf.Clamp(sensitivityX, minX, maxX);
        float tempY = Mathf.Clamp(sensitivityY, minY, maxY);



        if (Mathf.Abs(Input.GetAxis("Zoom")) > 0)
        {
            tempX -= Input.GetAxis("Zoom") * Time.deltaTime * (maxX - minX);
            tempY -= Input.GetAxis("Zoom") * Time.deltaTime * (maxY - minY);
        }

        if(Input.GetAxis("RightTrigger") < 0)
        {
            sensitivityX = tempX;
            sensitivityY = tempY;
        }
        else
        {
            sensitivityX = maxX;
            sensitivityY = maxY;
        }

        #region FPS Camera Stuff
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivityX * smoothing, sensitivityY * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseL += smoothV;
        mouseL.y = Mathf.Clamp(mouseL.y, -90f, 90f);
        transform.localRotation = Quaternion.AngleAxis(mouseL.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseL.x, character.transform.up);
        #endregion

        CameraBob();
    }


    // Camera bob script found here: https://answers.unity.com/questions/283086/headbobber-script-in-c.html
    void CameraBob()
    {

        bobSpeed = Mathf.Min(maxBobSpeed, playerVelocity / playerMaxVelocity);
        float waveSlice = 0f;
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");

        if(Mathf.Abs(Vertical)==0 && Mathf.Abs(Horizontal)==0)
        {
            timer = 0;
        }
        else
        {
            waveSlice = Mathf.Sin(timer);
            timer = timer + bobSpeed;
             if(timer >Mathf.PI*2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if( waveSlice!= 0)
        {
            float translateChange = waveSlice * bobAmount;
            float totalAxes = Mathf.Abs(Horizontal) + Mathf.Abs(Vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            this.transform.localPosition = new Vector3 ( this.transform.localPosition.x, midpoint + translateChange, this.transform.localPosition.z);
        }
        else
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, midpoint, this.transform.localPosition.z);
        }
    }
}
