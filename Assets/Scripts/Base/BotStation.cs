using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(BotSpawner))]
public class BotStation : MonoBehaviour
{
    [SerializeField] private int _botCount;

    [SerializeField] private Transform _botPosition;

    private BotSpawner _spawner;
    private List<Bot> _availableBots;
    private List<Bot> _busyBots;
    private List<Resource> _acceptedTargetResources;
    private Coroutine _coroutine;

    public event Action<Resource> BotBroughtResource;

    public void Init()
    {
        _spawner = GetComponent<BotSpawner>();
        _availableBots = new List<Bot>();
        _busyBots = new List<Bot>();
        _acceptedTargetResources = new List<Resource>();

        _availableBots = _spawner.CreateBot(_botCount);
    }

    public void AcceptResources(Queue<Resource> resources)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SendBotToResources(resources));
    }

    private IEnumerator SendBotToResources(Queue<Resource> resources)
    {
        Resource resource;

        while (resources.Count > 0)
        {
            if (_availableBots.Count > 0)
            {
                resource = resources.Dequeue();

                if (!_acceptedTargetResources.Contains(resource))
                {
                    _acceptedTargetResources.Add(resource);
                    SetBotStatus(resource);
                }

            }

            yield return null;
        }
    }

    private void SetBotStatus(Resource resource)
    {
        if (resource.IsTaken == false)
        {
            Bot bot = _availableBots[0];

            _availableBots.Remove(bot);

            _busyBots.Add(bot);

            bot.WalkToTarget(resource.transform);

            bot.BotReturn += AcceptBot;
        }
    }

    private void AcceptBot(Bot bot)
    {
        _busyBots.Remove(bot);
        _availableBots.Add(bot);

        if (bot.TakenResource != null)
        {
            BotBroughtResource?.Invoke(bot.TakenResource);
        }

        bot.BotReturn -= AcceptBot;
    }
}
