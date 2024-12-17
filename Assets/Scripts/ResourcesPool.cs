using System.Collections.Generic;
using UnityEngine;

public class ResourcesPool : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;

    private Queue<Resource> _resources = new Queue<Resource>();

    public Resource GetResource()
    {
        Resource resource;

        if (_resources.Count > 0)
        {
           resource =  _resources.Dequeue();
        }
        else
        {
            resource = CreateResource();
        }

        resource.OnResourceDelivered += TakeResource;

        return resource;
    }

    private Resource CreateResource()
    {
        return Instantiate(_resourcePrefab,transform);
    }

    public void TakeResource(Resource resource)
    {
        _resources.Enqueue(resource);
        resource.OnResourceDelivered -= TakeResource;
        resource.gameObject.SetActive(false);
    }
}
