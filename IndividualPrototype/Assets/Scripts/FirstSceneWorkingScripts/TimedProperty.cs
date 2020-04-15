using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedProperty : MonoBehaviour
{
    public UnityEvent TimedEvent;

    public bool activate = false;

    private void Update()
    {
        if(activate ==true)
        {
            TimedEvent.Invoke();
        }
    }

}
