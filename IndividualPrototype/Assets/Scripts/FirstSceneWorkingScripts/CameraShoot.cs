using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Camera heldCamera;
    [SerializeField] float FOVScaler, minFOV, maxFOV, fovDefault;
    [SerializeField] GameObject cameraObject;
    [SerializeField] AudioSource playerAudioSource;
    [SerializeField] AudioClip readPropertySFX, applyPropertySFX, enemyKill;

    [SerializeField] GameObject bouncyIcon, massIcon, movingIcon, electricityIcon, timingIcon;

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
        ClearStoredProperties();
        myStoredProperties.StoredTagsList.Clear();
        camerashootRay = new Ray(heldCamera.transform.position, heldCamera.transform.forward);
        RaycastHit rH;
        if (Physics.Raycast(camerashootRay, out rH, 100f))
        {
            if(rH.collider.gameObject.GetComponent<NewPropertyTagContainer>()!=null)
            {
                var tagReading = rH.collider.gameObject.GetComponent<NewPropertyTagContainer>();
                GameObject readObject = rH.collider.gameObject;
                
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
                            massIcon.SetActive(true);
                            break;

                        case "Electricity":
                            myStoredProperties.Electricity = 10f;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            electricityIcon.SetActive(true);
                            break;

                        case "Timed":
                            myStoredProperties.Timed = true;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            timingIcon.SetActive(true);
                            break;


                        case "Moving":
                            myStoredProperties.Moving = true;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            movingIcon.SetActive(true);
                            break;

                        case "Bouncy":
                            myStoredProperties.Bounce = true;
                            playerAudioSource.PlayOneShot(readPropertySFX);
                            bouncyIcon.SetActive(true);
                            break;
                    }
                }
            }

            #region Old Reading stuff
            /*
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
            */
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
                var objectApplyTags = rH.collider.gameObject.GetComponent<NewPropertyTagContainer>();
                var objectToApply = rH.collider.gameObject;
                foreach (string storedTag in myStoredProperties.StoredTagsList)
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
                                    if(myStoredProperties.Bounce == true&& rH.collider.gameObject.GetComponent<bouncePlatformScript>().activeText!=null)
                                    {
                                        rH.collider.gameObject.GetComponent<bouncePlatformScript>().activeText.text = "Active";
                                    }
                                    playerAudioSource.PlayOneShot(readPropertySFX);
                                    break;
                            }
                        }
                    }
                        
                    
                }
                ClearStoredProperties();
                playerAudioSource.PlayOneShot(applyPropertySFX);
            } 

            if (rH.collider.gameObject.tag =="Enemy")
            {
                AudioSource enemyAudio = rH.collider.gameObject.GetComponent<AudioSource>();
                for (int i = 0; i < myStoredProperties.StoredTagsList.Count; i++)
                {

                    switch (myStoredProperties.StoredTagsList[i])
                    {
                        case "Mass":
                            if (1 <= myStoredProperties.Mass)
                            {
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
                            break;

                        case "Electricity":
                            enemyAudio.PlayOneShot(enemyKill);
                            rH.collider.gameObject.GetComponentInChildren<Animator>().SetTrigger("Death");
                            Destroy(rH.collider.gameObject, 0.8f);
                            break;

                        case "Timed":
                            StartCoroutine(kill(rH.collider.gameObject, enemyAudio));
                            playerAudioSource.PlayOneShot(applyPropertySFX);
                            break;
                    }
                }

                ClearStoredProperties();
            }
            #region Old Apply stuff
            /*
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
            */
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

        bouncyIcon.SetActive(false);
        movingIcon.SetActive(false);
        massIcon.SetActive(false);
        electricityIcon.SetActive(false);
        timingIcon.SetActive(false);
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
