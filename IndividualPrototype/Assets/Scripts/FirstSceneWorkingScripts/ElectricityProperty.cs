using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class ElectricityProperty : MonoBehaviour
{
    public float powerLevel;
    [SerializeField] float powerThreshold;
    [SerializeField] TMP_Text INDICATOR;
    [SerializeField] Image powerImage;
    public UnityEvent activatePower;
    
    void FixedUpdate()
    {
        float pL = Mathf.Clamp(powerLevel / powerThreshold, 0f, 1f);
        if(powerLevel>= powerThreshold)
        {
            activatePower.Invoke();
        }
        if (INDICATOR != null)
        {
            INDICATOR.text = "Power Level: " + (int)((pL)*100)+"%";
        }
        if(powerImage!=null)
        {
            powerImage.fillAmount = pL;
        }
    }
}
