using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ResourcesStorage : MonoBehaviour
{
    public int CristallCount { get; private set; }
    public int SteelCount{get; private set;}

    public event Action<int,int> ResourceAdded;

    public void Init()
    {
        CristallCount = 0;
        SteelCount = 0;
    }

    public void AddResource(Resource resource)
    {
        if (resource is Cristall)
        {
            CristallCount++;
        }
        else if (resource is Steel)
        {
            SteelCount++;
        }

        ResourceAdded?.Invoke(CristallCount,SteelCount);
    } 
}
