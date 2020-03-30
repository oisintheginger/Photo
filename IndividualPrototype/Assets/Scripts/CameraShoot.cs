using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraShoot : MonoBehaviour
{
    [SerializeField] Camera heldCamera;
    [SerializeField] GameObject cameraObject;

    [SerializeField] Transform aimingPos, notAimingPos;
    bool Aiming;
    //public RenderTexture tex;
    [SerializeField] float shootWait =2f;
    float shootTimer;
    Ray camerashootRay;

    private void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/ScreenShots");
        shootTimer = shootWait;
    }
    private void Update()
    {
        if(Input.GetAxis("RightTrigger")<0)
        {
            Aiming = true;
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            Aiming = false;
        }

        MoveCameraToPosition(Aiming);
    }

    void MoveCameraToPosition(bool isAiming)
    {
        if(isAiming)
        {
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, aimingPos.position, 5f * Time.deltaTime);
        }
        else
        {
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, notAimingPos.position, 5f * Time.deltaTime);
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
                //SaveTexturePNG(toTexture2D(heldCamera.targetTexture));
                Debug.Log(Application.dataPath);
            }
        }
    }

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


}
