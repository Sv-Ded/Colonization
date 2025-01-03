using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _takerPosition;

    private float _distanceToTake = 1;
    private NavMeshAgent _agent;
    private Transform _startPosition;
    private Transform _walkTarget;
    private Resource _targetResource;
    private bool _isCarriesBase = false;
    private BuildedBase _baseTemp = null;

    public Resource TakenResource { get; private set; }

    public event Action<Bot> BotReturn;
    public event Action<Bot> BotJoinedNewBase;

    public void Init(Transform station)
    {
        _agent = GetComponent<NavMeshAgent>();
        _startPosition = station.transform;
    }

    private void Update()
    {
        if (_walkTarget != null)
        {
            _agent.SetDestination(_walkTarget.position);

            if ((transform.position - _walkTarget.position).sqrMagnitude < _distanceToTake)
            {
                if (_walkTarget.TryGetComponent(out Resource resource))
                {
                    TakeResource(resource);
                }
                else if (_walkTarget.TryGetComponent(out BuildedBase newBase) && newBase.IsBuilded==false)
                {
                    TakeBase(newBase);
                }
                else if (_isCarriesBase)
                {
                    SetBase(_baseTemp);
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
        if (target.TryGetComponent(out Resource resource))
        {
            _walkTarget = resource.transform;
            _targetResource = resource;
        }
        else
        {
            _walkTarget = target;
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

    private void TakeBase(BuildedBase buildedBase)
    {
        _isCarriesBase = true;
        buildedBase.transform.SetParent(transform, false);
        buildedBase.transform.localPosition = _takerPosition.localPosition;

        _baseTemp = buildedBase;
        WalkToTarget(buildedBase.BuildingPosition);
    }

    private void SetBase(BuildedBase buildedBase)
    {
        _isCarriesBase = false;
        buildedBase.transform.parent = null;
        buildedBase.transform.position = buildedBase.BuildingPosition.position;
        buildedBase.transform.rotation = buildedBase.BuildingPosition.rotation;
        buildedBase.BecomeBuilded();
        _baseTemp = null;

        BotJoinedNewBase?.Invoke(this);
        buildedBase.AcceptNewBot(this);
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
