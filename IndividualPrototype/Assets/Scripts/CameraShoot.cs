using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraShoot : MonoBehaviour
{
    [SerializeField]Camera heldCamera;
    public RenderTexture tex;
    Ray camerashootRay;

    private void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/ScreenShots");
    }
    private void Update()
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
                SaveTexturePNG(toTexture2D(heldCamera.targetTexture));
                Debug.Log(Application.dataPath);
            }
        }
    }
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
