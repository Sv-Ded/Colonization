using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BotStation), typeof(ResourcesStorage), typeof(ResourcesScanner))]
[RequireComponent (typeof(StorageView))]
public class Base : MonoBehaviour
{
    private BotStation _botStation;
    private ResourcesScanner _scanner;
    private ResourcesStorage _storage;
    private StorageView _storageView;

    private void Awake()
    {
        _botStation = GetComponent<BotStation>();
        _scanner = GetComponent<ResourcesScanner>();
        _storage = GetComponent<ResourcesStorage>();
        _storageView = GetComponent<StorageView>();
    }

    private void OnEnable()
    {
        _scanner.ResourcesFounded +=_botStation.AcceptResources;

        _botStation.BotBroughtResource += _storage.AddResource;

        _storage.ResourceAdded += _storageView.UpdateText;
    }

    private void Start()
    {
        _botStation.Init();
        _scanner.Init();
        _storage.Init();
    }

    private void OnDisable()
    {
        _scanner.ResourcesFounded -= _botStation.AcceptResources;

        _botStation.BotBroughtResource -= _storage.AddResource;

        _storage.ResourceAdded -= _storageView.UpdateText;
    }
}
