using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[System.Serializable]
public class StoredProperties
{
    public bool Timed = false;
    public float Mass = 0f;
    public float Electricity =0f;
    public bool Moving = false;
    public bool Bounce;
    public string storedGameObjectTag;
    public List<string> StoredTagsList = new List<string>();
}

public class CameraShoot : MonoBehaviour
{
    [SerializeField] TMP_Text StoredPropertyUI;
    [SerializeField] Camera heldCamera;
    [SerializeField] float FOVScaler, minFOV, maxFOV, fovDefault;
    [SerializeField] GameObject cameraObject;
    [SerializeField] AudioSource playerAudioSource;
    [SerializeField] AudioClip readPropertySFX, applyPropertySFX, enemyKill;


    public StoredProperties myStoredProperties;


    [SerializeField] Transform aimingPos, notAimingPos, cameraShootRayPos;
    bool Aiming;
    //public RenderTexture tex;
    [SerializeField] float shootWait = 2f;
    float shootTimer;
    Ray camerashootRay;

    private void Start()
    {
        playerAudioSource = this.gameObject.GetComponent<AudioSource>();
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
        StoredPropertyUI.text = "Storing "+ myStoredProperties.storedGameObjectTag;
        fovDefault = Mathf.Clamp(fovDefault, minFOV, maxFOV);
        heldCamera.GetComponent<Camera>().fieldOfView = fovDefault;
        
        
        if (Input.GetAxis("RightTrigger") < 0)
        {
            if (Mathf.Abs(Input.GetAxis("Zoom")) > 0)
            {
                fovDefault += FOVScaler * -Input.GetAxis("Zoom") * Time.deltaTime;
            }

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
            fovDefault = maxFOV;
        }

        MoveCameraToPosition(Aiming);
    }

    void ReadProperties()
    {
        camerashootRay = new Ray(heldCamera.transform.position, heldCamera.transform.forward);
        RaycastHit rH;
        if (Physics.Raycast(camerashootRay, out rH, 100f))
        {
            if(rH.collider.gameObject.GetComponent<NewPropertyTagContainer>()!=null)
            {
                var tagReading = rH.collider.gameObject.GetComponent<NewPropertyTagContainer>();
                GameObject readObject = rH.collider.gameObject;
                myStoredProperties.StoredTagsList.Clear();
                foreach(string tag in tagReading.readTags)
                {
                    myStoredProperties.StoredTagsList.Add(tag);
                }
                

                foreach (string tag in tagReading.readTags)
                {
                    switch (tag)
                    {
                        case "Mass":
                            myStoredProperties.Mass = readObject.GetComponent<Rigidbody>().mass;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            Debug.Log("New Reading: Mass " + myStoredProperties.Mass);
                            break;

                        case "Electricity":
                            myStoredProperties.Electricity = 10f;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            Debug.Log("Read: Electricity " + myStoredProperties.Electricity);
                            break;

                        case "Timed":
                            myStoredProperties.Timed = true;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            Debug.Log("Read: Timed " + myStoredProperties.Timed);
                            break;


                        case "Moving":
                            myStoredProperties.Moving = true;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            Debug.Log("Read: Moving " + myStoredProperties.Moving);
                            break;

                        case "Bouncy":
                            myStoredProperties.Bounce = true;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            break;
                    }
                }
            }

            #region Old Reading stuff
            var objecttoRead = rH.collider.gameObject;
            switch (rH.collider.gameObject.tag)
            {
                case "Mass":
                    myStoredProperties.Mass = objecttoRead.GetComponent<Rigidbody>().mass;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    playerAudioSource.PlayOneShot(readPropertySFX);
                    break;

                case "Electricity":
                    myStoredProperties.Electricity = 10f;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    playerAudioSource.PlayOneShot(readPropertySFX);
                    break;

                case "Timed":
                    myStoredProperties.Timed = true;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    playerAudioSource.PlayOneShot(readPropertySFX);
                    break;


                case "Moving":
                    myStoredProperties.Moving = true;
                    myStoredProperties.storedGameObjectTag = objecttoRead.tag;
                    playerAudioSource.PlayOneShot(readPropertySFX);
                    break;
            }
            #endregion
        }
    }

    void ApplyProperties()
    {
        camerashootRay = new Ray(heldCamera.transform.position, heldCamera.transform.forward);
        RaycastHit rH;
        if (Physics.Raycast(camerashootRay, out rH, 100f))
        {

            if(rH.collider.gameObject.GetComponent<NewPropertyTagContainer>()!=null)
            {
                Debug.Log("Got here");
                var objectApplyTags = rH.collider.gameObject.GetComponent<NewPropertyTagContainer>();
                var objectToApply = rH.collider.gameObject;
                foreach (string storedTag in myStoredProperties.StoredTagsList)
                {
                    
                    if (objectApplyTags.applyTags.Count > 0)
                    {
                        for (int i = 0; i < objectApplyTags.applyTags.Count; i++)
                        {
                            if (storedTag == objectApplyTags.applyTags[i])
                            {
                                switch (objectApplyTags.applyTags[i])
                                {
                                    case "Mass":
                                        objectToApply.GetComponent<Rigidbody>().mass = myStoredProperties.Mass;
                                        
                                        break;

                                    case "Electricity":
                                        objectToApply.GetComponent<ElectricityProperty>().powerLevel += myStoredProperties.Electricity;
                                        
                                        break;

                                    case "Timed":
                                        objectToApply.GetComponent<TimedProperty>().activate = myStoredProperties.Timed;
                                        
                                        break;


                                    case "Moving":
                                        objectToApply.GetComponent<FollowPath>().Active = myStoredProperties.Moving;
                                        
                                        break;
                                    case "Bouncy":
                                        rH.collider.GetComponent<bouncePlatformScript>().isBouncy = myStoredProperties.Bounce;
                                        playerAudioSource.PlayOneShot(readPropertySFX);
                                        break;
                                }
                            }
                        }
                        
                    }
                }
                ClearStoredProperties();
                playerAudioSource.PlayOneShot(applyPropertySFX);
            }
            #region Old Apply stuff
            var objectToAlter = rH.collider.gameObject;
            if (myStoredProperties.storedGameObjectTag + "Apply" == rH.collider.gameObject.tag)
            {
                switch (myStoredProperties.storedGameObjectTag)
                {
                    case "Mass":
                        objectToAlter.GetComponent<Rigidbody>().mass = myStoredProperties.Mass;
                        ClearStoredProperties();
                        break;

                    case "Electricity":
                        objectToAlter.GetComponent<ElectricityProperty>().powerLevel += myStoredProperties.Electricity;
                        ClearStoredProperties();
                        break;

                    case "Timed":
                        objectToAlter.GetComponent<TimedProperty>().activate = myStoredProperties.Timed;
                        ClearStoredProperties();
                        break;


                    case "Moving":
                        objectToAlter.GetComponent<FollowPath>().Active = myStoredProperties.Moving;
                        ClearStoredProperties();
                        break;
                }
                playerAudioSource.PlayOneShot(applyPropertySFX);
            }
            if(rH.collider.tag =="Enemy")
            {
                AudioSource enemyAudio = rH.collider.gameObject.GetComponent<AudioSource>();
                switch (myStoredProperties.storedGameObjectTag)
                {
                    case "Mass":
                        if(1 <= myStoredProperties.Mass)
                        {
                            Debug.Log("less than");
                            rH.collider.gameObject.GetComponent<EnemyScript>().aggressiveSpeed *= 0.25f;
                            rH.collider.gameObject.GetComponent<EnemyScript>().passiveSpeed *= 0.25f;
                            rH.collider.gameObject.GetComponent<EnemyScript>().ShootingTimer *= 2f;
                        }
                        else
                        {
                            
                            rH.collider.gameObject.GetComponent<EnemyScript>().aggressiveSpeed *= 1.5f;
                            rH.collider.gameObject.GetComponent<EnemyScript>().passiveSpeed *= 1.5f;
                            rH.collider.gameObject.GetComponent<EnemyScript>().ShootingTimer *= 0.5f;
                        }
                        enemyAudio.PlayOneShot(enemyKill);
                        ClearStoredProperties();
                        break;

                    case "Electricity":
                        enemyAudio.PlayOneShot(enemyKill);
                        rH.collider.gameObject.GetComponentInChildren<Animator>().SetTrigger("Death");
                        Destroy(rH.collider.gameObject, 0.8f);
                        ClearStoredProperties();
                        break;

                    case "Timed":
                        StartCoroutine(kill(rH.collider.gameObject, enemyAudio));
                        playerAudioSource.PlayOneShot(applyPropertySFX);
                        //Kill enemys after time limit
                        break;
                }
            }

            #endregion 
        }
    }

    IEnumerator kill(GameObject kill, AudioSource enemyAudio)
    {
        yield return new WaitForSeconds(3f);
        enemyAudio.PlayOneShot(enemyKill);
        kill.GetComponentInChildren<Animator>().SetTrigger("Death");
        Destroy(kill, 0.7f);
    }
    void ClearStoredProperties()
    {
        myStoredProperties.Mass = 0f;
        myStoredProperties.Electricity = 0f;
        myStoredProperties.Moving = false;
        myStoredProperties.Timed = false;
        myStoredProperties.storedGameObjectTag = null;
        if (myStoredProperties.StoredTagsList != null)
        {
            myStoredProperties.StoredTagsList.Clear();
        }
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
