using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricityProperty : MonoBehaviour
{
    // Start is called before the first frame update
    public float powerLevel;
    [SerializeField] float powerThreshold;

    public UnityEvent activatePower;
    
    void Update()
    {
        if(powerLevel>= powerThreshold)
        {
            activatePower.Invoke();
        }
    }
}
