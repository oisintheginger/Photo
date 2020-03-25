using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera mc;
    [SerializeField] Material material;

    void Start()
    {
        if (mc.targetTexture != null)
        {
            mc.targetTexture.Release();
        }

        mc.targetTexture = new RenderTexture(Screen.width, Screen.height, 40);
        mc.targetTexture.useDynamicScale=true;
        material.mainTexture = mc.targetTexture;
    }
}
