using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    public List<Transform> pathPoints;
    private void Start()
    {
        for(int i =0; i< this.transform.childCount; i++)
        {
            pathPoints.Add(this.transform.GetChild(i));
        }
    }
}
