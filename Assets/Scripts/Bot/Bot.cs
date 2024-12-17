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
    [SerializeField] private Resource _targetResource;

    [field: SerializeField] public Resource TakenResource { get; private set; }

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
        if (_walkTarget != null)
        {
            if ((transform.position - _walkTarget.position).sqrMagnitude < 25)
            {
                if (_walkTarget.TryGetComponent(out Resource resource))
                {
                    TakeResource(resource);
                }
                else
                {
                    SetNewTarget();
                }
            }

            if (_targetResource != null && _targetResource.IsTaken && TakenResource == null)
            {
                SetNewTarget();
            }
        }
    }

    public void WalkToTarget(Transform target)
    {
        _agent.SetDestination(target.position);

        if (target.TryGetComponent(out Resource resource))
        {
            _walkTarget = resource.transform;
            _targetResource = resource;
        }
        else
        {
            _walkTarget = _startPosition;
        }
    }

    private void TakeResource(Resource resource)
    {
        resource.BecomeTaken();

        resource.transform.SetParent(transform, false);

        resource.transform.localPosition = _takerPosition.localPosition;

        TakenResource = resource;

        WalkToTarget(_startPosition);
    }

    private void SetNewTarget()
    {
        WalkToTarget(_startPosition);
        _walkTarget = null;
        _targetResource = null;
        BotReturn?.Invoke(this);

        if (TakenResource != null)
        {
            TakenResource.transform.parent = null;
            TakenResource.BecomeDelivered();
            TakenResource = null;
        }
    }
}
