using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDeath : MonoBehaviour
{
   public void TimedContainer(GameObject objectToDestroy)
    {
        StartCoroutine(Timer(objectToDestroy));
    }
    IEnumerator Timer(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(3f);
        GameObject.Destroy(objectToDestroy);
    }
}
