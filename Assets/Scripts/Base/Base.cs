using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BotStation), typeof(ResourcesStorage), typeof(ResourcesScanner))]
public class Base : MonoBehaviour
{
    private BotStation _botStation;
    private ResourcesScanner _scanner;
    private ResourcesStorage _storage;

    private void Awake()
    {
        _botStation = GetComponent<BotStation>();
        _scanner = GetComponent<ResourcesScanner>();
        _storage = GetComponent<ResourcesStorage>();
    }

    private void OnEnable()
    {
        _scanner.ResourcesFounded +=_botStation.SendBotToResources;

        _botStation.BotBroughtResource += _storage.AddResource;
    }

    private void Start()
    {
        _botStation.Init();
        _scanner.Init();
        _storage.Init();
    }

    private void OnDisable()
    {
        _scanner.ResourcesFounded -= _botStation.SendBotToResources;

        _botStation.BotBroughtResource -= _storage.AddResource;
    }
}
