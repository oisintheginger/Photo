using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class StoredProperties
{
    public bool Timed = false;
    public float Mass = 0f;
    public float Electricity =0f;
    public bool Moving = false;

    public string storedGameObjectTag;
}

public class CameraShoot : MonoBehaviour
{
    [SerializeField] Camera heldCamera;
    [SerializeField] float FOVScaler, minFOV, maxFOV, fovDefault;
    [SerializeField] GameObject cameraObject;

    public StoredProperties myStoredProperties;


    [SerializeField] Transform aimingPos, notAimingPos, cameraShootRayPos;
    bool Aiming;
    //public RenderTexture tex;
    [SerializeField] float shootWait = 2f;
    float shootTimer;
    Ray camerashootRay;

    private void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/ScreenShots");
        myStoredProperties = new StoredProperties();
        if(fovDefault==0)
        {
            fovDefault = 60f;
        }
        shootTimer = shootWait;
    }
    
    private void Update()
    {
        fovDefault = Mathf.Clamp(fovDefault, minFOV, maxFOV);
        heldCamera.GetComponent<Camera>().fieldOfView = fovDefault;
        if (Mathf.Abs(Input.GetAxis("Zoom"))>0)
        {
            
            fovDefault += FOVScaler * -Input.GetAxis("Zoom") * Time.deltaTime;
        }
        
        if (Input.GetAxis("RightTrigger") < 0)
        {
            
            Aiming = true;
            if (Input.GetButtonDown("Read"))
            {
                ReadProperties();
            }
            if (Input.GetButtonDown("Apply"))
            {
                ApplyProperties();
            }
        }
        else
        {
            Aiming = false;
        }

        MoveCameraToPosition(Aiming);
    }

    void ReadProperties()
    {
        camerashootRay = new Ray(heldCamera.transform.position, heldCamera.transform.forward);
        RaycastHit rH;
        if (Physics.Raycast(camerashootRay, out rH, 100f))
        {
            var objecttoRead = rH.collider.gameObject;
            switch (rH.collider.gameObject.tag)
            {
                case "Mass":
                    myStoredProperties.Mass = objecttoRead.GetComponent<Rigidbody>().mass;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    Debug.Log("Read: Mass " + myStoredProperties.Mass);
                    break;

                case "Electricity":
                    myStoredProperties.Electricity = 10f;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    Debug.Log("Read: Electricity " + myStoredProperties.Electricity);
                    break;

                case "Timed":
                    myStoredProperties.Timed = true;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    Debug.Log("Read: Timed " + myStoredProperties.Timed);
                    break;


                case "Moving":
                    myStoredProperties.Moving = true;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    Debug.Log("Read: Moving " + myStoredProperties.Moving);
                    break;
            }
        }
    }

    void ApplyProperties()
    {
        camerashootRay = new Ray(heldCamera.transform.position, heldCamera.transform.forward);
        RaycastHit rH;
        if (Physics.Raycast(camerashootRay, out rH, 100f))
        {
            var objectToAlter = rH.collider.gameObject;
            if (myStoredProperties.storedGameObjectTag + "Apply" == rH.collider.gameObject.tag)
            {
                switch (myStoredProperties.storedGameObjectTag)
                {
                    case "Mass":
                        objectToAlter.GetComponent<Rigidbody>().mass = myStoredProperties.Mass;
                        ClearStoredProperties();
                        Debug.Log("Applied: Mass " + myStoredProperties.Mass);
                        break;

                    case "Electricity":
                        objectToAlter.GetComponent<ElectricityProperty>().powerLevel += myStoredProperties.Electricity;
                        ClearStoredProperties();
                        Debug.Log("Applied: Electricity " + myStoredProperties.Electricity);
                        break;

                    case "Timed":
                        objectToAlter.GetComponent<TimedProperty>().activate = myStoredProperties.Timed;
                        ClearStoredProperties();
                        Debug.Log("Applied: Timed " + myStoredProperties.Timed);
                        break;


                    case "Moving":
                        objectToAlter.GetComponent<FollowPath>().Active = myStoredProperties.Moving;
                        ClearStoredProperties();
                        Debug.Log("Applied: Moving " + myStoredProperties.Moving);
                        break;
                }
            }
            if(rH.collider.tag =="Enemy")
            {
                switch (myStoredProperties.storedGameObjectTag)
                {
                    case "Mass":
                        //Decrease Enemy Speed
                        Debug.Log("Applied: Mass " + myStoredProperties.Mass);
                        break;

                    case "Electricity":
                        //Electrocute Enemy
                        break;

                    case "Timed":
                        //Kill enemys after time limit
                        Debug.Log("Applied: Timed " + myStoredProperties.Timed);
                        break;
                }
            }
        }
    }


    void ClearStoredProperties()
    {
        myStoredProperties.Mass = 0f;
        myStoredProperties.Electricity = 0f;
        myStoredProperties.Moving = false;
        myStoredProperties.Timed = false;
        myStoredProperties.storedGameObjectTag = null;
    }
    void MoveCameraToPosition(bool isAiming)
    {
        if (isAiming)
        {
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, aimingPos.position, 5f * Time.deltaTime);
        }
        else
        {
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, notAimingPos.position, 5f * Time.deltaTime);
        }
    }

    /* List of Effects and Properties
     * list of properties:              list of effects:
     * -Timed                         -when applied to enemies, causes them to die after 5 seconds, bool type
     *                                  
     * -Static                          -when applied to enemies or other rigidbodies it causes them to become static, bool type
     * 
     * -Mass                            -swap mass of object, float type
     * 
     * -Electricity                     -when applied to electronics, it can power them, float type
     * 
     * -Moving                          -when applied to a static object, it can cause it to move in a patrol, i.e. moving platforms, bool type
     */


    
        #region Saving Pictures
        //Sources for saving rendertexture data to a png file
        //https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d 
        //https://answers.unity.com/questions/862685/saving-screenshot-and-using-it-as-texture-at-runti.html

        string NameForScreenshot(int i)
        {
            return string.Format(Application.dataPath + "/ScreenShots/{0}.png", i);
        }
        Texture2D toTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        void SaveTexturePNG(Texture2D tex)
        {
            byte[] bytes = tex.EncodeToPNG();
            Object.Destroy(tex);
            string filename = NameForScreenshot(1);
            File.WriteAllBytes(filename, bytes);
        }
        #endregion

    
}
