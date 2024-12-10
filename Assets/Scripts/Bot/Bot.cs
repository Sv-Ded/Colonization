using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _takerPosition;

    private NavMeshAgent _agent;
    private Transform _startPosition;
    private Transform _walkTarget;
    private bool _isActive = false;

    public Resource TakenResource { get; private set; }

    public event Action<Bot> BotReturn;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Init(Transform station)
    {
        _startPosition = station.transform;
    }

    private void Update()
    {
        if (_isActive)
        {
            if ((transform.position - _walkTarget.position).sqrMagnitude < 25)
            {
                if (_walkTarget.TryGetComponent(out Resource resource))
                {
                    TakeResource(resource);
                }
                else
                {
                    ReturnToBase();
                }
            }
        }
    }

    public void WalkToTarget(Transform target)
    {
        _isActive = true;

        //_agent.SetDestination(target.position);

        if (target.TryGetComponent(out Resource resource))
        {
            _walkTarget = resource.transform;
        }
        else
        {
            _walkTarget = _startPosition;
        }
    }

    private void TakeResource(Resource resource)
    {
        resource.BecomeTaken();

        resource.transform.position = _takerPosition.position;

        resource.transform.SetParent(transform, false);

        TakenResource = resource;

        WalkToTarget(_startPosition);
    }

    public void ReturnToBase()
    {
        BotReturn?.Invoke(this);
        _isActive = false;

        if (TakenResource != null)
        {
            TakenResource.transform.parent = null;
            TakenResource.gameObject.SetActive(false);
            TakenResource = null;
            _walkTarget = null;
        }
    }
}
