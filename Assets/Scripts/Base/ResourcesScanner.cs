using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesScanner : MonoBehaviour
{
    [SerializeField] private float _scanDelay;
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourcesMask;

    private WaitForSeconds _scanBreak;
    private Coroutine _coroutine;

    private Queue<Resource> _resourcesOnMap = new();

    public event Action<Queue<Resource>> ResourcesFounded;

    public void Init()
    {
        _scanBreak = new WaitForSeconds(_scanDelay);

        _coroutine = StartCoroutine(ScanCoroutine());
    }

    private IEnumerator ScanCoroutine()
    {
        while (enabled)
        {
            GetResourcesOnMap();

            ResourcesFounded.Invoke(_resourcesOnMap);

            yield return _scanBreak;
        }
    }

    private void GetResourcesOnMap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourcesMask);

        _resourcesOnMap.Clear();

        foreach (Collider collider in colliders)
        {
            collider.TryGetComponent(out Resource resource);

            if (!resource.IsTaken)
            {
                _resourcesOnMap.Enqueue(resource);
            }
        }
    }
}