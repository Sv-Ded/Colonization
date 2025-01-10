using System;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    private Transform _startTransform;

    public bool IsTaken { get; private set; }

    public event Action<Resource> OnResourceDelivered;

    private void Awake()
    {
        _startTransform = transform;
    }

    public void Init(Vector3 position)
    {
        IsTaken = false;

        transform.localScale = _startTransform.localScale;

        transform.position = position;

        gameObject.SetActive(true);
    }

    public void BecomeTaken()
    {
        IsTaken = true;
    }

    public void BecomeDelivered()=> OnResourceDelivered?.Invoke(this);
}
