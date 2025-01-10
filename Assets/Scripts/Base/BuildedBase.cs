using System;
using UnityEngine;

public class BuildedBase : OriginBase
{
    public Transform BuildingPosition { get; private set; }

    public bool IsBuilded { get; private set; } = false;

    public event Action<BuildedBase> OnBuilded;

    public void Init(Transform basePosition)
    {
        DisableText();
        BuildingPosition = basePosition;
    }

    public void BecomeBuilded()
    {
        IsBuilded = true;
        OnBuilded?.Invoke(this);

        EnableText();
    }
}
