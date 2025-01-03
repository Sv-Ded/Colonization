using UnityEngine;

[RequireComponent(typeof(BotStation), typeof(ResourcesStorage), typeof(ResourcesScanner))]
[RequireComponent(typeof(StorageView), typeof(BaseStateMachine))]
public class OriginBase : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private int _botCount;

    private BotStation _botStation;
    private ResourcesScanner _scanner;
    private ResourcesStorage _storage;
    private StorageView _storageView;
    private BaseStateMachine _stateMachine;

    public Flag Flag { get; private set; }

    private void Awake()
    {
        _botStation = GetComponent<BotStation>();
        _scanner = GetComponent<ResourcesScanner>();
        _storage = GetComponent<ResourcesStorage>();
        _storageView = GetComponent<StorageView>();
        _stateMachine = GetComponent<BaseStateMachine>();

        Flag = Instantiate(_flagPrefab, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        _scanner.ResourcesFounded += _botStation.AcceptResources;

        _botStation.BotBroughtResource += _storage.AddResource;

        _storage.ResourcesCountChanged += _storageView.UpdateText;
        _storage.ResourcesCountChanged += _stateMachine.Run;

        _stateMachine.ResourcesUsed += _storage.RemoveResources;
    }

    private void Start()
    {
        _botStation.Init(_botCount);
        _scanner.Init();
        _stateMachine.Init(_botStation);
    }

    private void OnDisable()
    {
        _scanner.ResourcesFounded -= _botStation.AcceptResources;

        _botStation.BotBroughtResource -= _storage.AddResource;

        _storage.ResourcesCountChanged -= _storageView.UpdateText;
        _storage.ResourcesCountChanged -= _stateMachine.Run;

        _stateMachine.ResourcesUsed -= _storage.RemoveResources;
    }

    public void SetFlagPosition(Vector3 selectedPosition)
    {
        if (_botStation.BotCount > 1)
        {
            Flag.transform.position = selectedPosition;

            _stateMachine.SetBaseCreatorState();
        }
    }

    public void AcceptNewBot(Bot bot)
    {
        _botStation.AddBot(bot);
    }
}