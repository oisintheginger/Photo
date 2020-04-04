using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ReadProperties 
{
    public bool Expires;
    public bool Static;
    public bool Mass;
    public bool Electricity;
    public bool Moving;
    public float ExpireTimer;
}

[System.Serializable]
public struct CompatableProperties
{
    public bool Expires;
    public bool Static;
    public bool Mass;
    public bool Electricity;
    public bool Moving;
}



public class ObjectProperties : MonoBehaviour
{
    public ReadProperties myReadableProperties;
    public CompatableProperties myCompatableProperties;

    private void Awake()
    { 

    }
}
