using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    public bool IsTaken { get; private set; } = false;

    public void BecomeTaken()
    {
        IsTaken = true;
    }
}
