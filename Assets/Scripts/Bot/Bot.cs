using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _takerPosition;
    [SerializeField] private BuiltBase _buildedBase;

    private float _distanceToTake = 1;
    private NavMeshAgent _agent;
    private Transform _startPosition;
    private Transform _walkTarget;
    private Resource _targetResource;
    private bool _isBuildBase = false;

    public Resource TakenResource { get; private set; }

    public event Action<Bot> Return;
    public event Action<Bot> BaseBuilt;

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
                else if (_isBuildBase)
                {
                    BuildBase();
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

    public void InitBaseBuilding()=> _isBuildBase = true;

    private void BuildBase()
    {
        BuiltBase builtBase = Instantiate(_buildedBase, _walkTarget.position, _walkTarget.rotation);

        BaseBuilt.Invoke(this);

        builtBase.AcceptNewBot(this);

        _isBuildBase = false;
    }

    private void SetNewTarget()
    {
        WalkToTarget(_startPosition);
        _walkTarget = null;
        _targetResource = null;
        Return?.Invoke(this);

        if (TakenResource != null)
        {
            TakenResource.transform.parent = null;
            TakenResource.BecomeDelivered();
            TakenResource = null;
        }
    }
}
