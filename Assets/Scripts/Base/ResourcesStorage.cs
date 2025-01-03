using System;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    public int CristallCount { get; private set; }
    public int SteelCount{get; private set;}

    public event Action<int,int> ResourcesCountChanged;

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

        ResourcesCountChanged?.Invoke(CristallCount,SteelCount);
    }
    
    public void RemoveResources(int cristallCount, int steelCount)
    {
        CristallCount -= cristallCount;
        SteelCount -= steelCount;

        ResourcesCountChanged?.Invoke(CristallCount,SteelCount);
    }
}
