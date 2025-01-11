using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(BotSpawner))]
public class BotStation : MonoBehaviour
{
    private BotSpawner _spawner;
    private List<Bot> _availableBots = new List<Bot>();
    private List<Bot> _busyBots = new List<Bot>();
    private List<Resource> _acceptedTargetResources =new List<Resource>();
    private Coroutine _coroutine;

    public int BotCount { get { return _availableBots.Count + _busyBots.Count; } }

    public event Action<Resource> BotBroughtResource;
    public event Action BaseBuilt;

    public void Init(int botCount)
    {
        _spawner = GetComponent<BotSpawner>();

        for (int i = 0; i < botCount; i++)
        {
            AddBot();
        }
    }

    public void AddBot()
    {
        Bot bot = _spawner.CreateBot();
        bot.Init(transform);

        _availableBots.Add(bot);
    }

    public void AddBot(Bot bot)
    {
        bot.Init(transform);
        _availableBots.Add(bot);
    }

    public void AcceptResources(Queue<Resource> resources)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SendBotToResources(resources));
    }

    public void SendBotToBuildBase(Transform position)
    {
        StartCoroutine(WaitAvailableBot(position));
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
                    SetBotStatus(_availableBots[0], resource.transform);
                }
            }

            yield return null;
        }
    }

    private IEnumerator WaitAvailableBot(Transform position)
    {
        while (_availableBots.Count == 0)
        {
            yield return null;
        }

        Bot bot = _availableBots[0];

        bot.BaseBuilt += RemoveBot;

        bot.InitBaseBuilding();

        SetBotStatus(bot, position);
    }

    private void SetBotStatus(Bot bot,Transform target)
    {
        _availableBots.Remove(bot);

        _busyBots.Add(bot);

        bot.WalkToTarget(target);

        bot.Return += AcceptBot;
    }

    private void AcceptBot(Bot bot)
    {
        _busyBots.Remove(bot);
        _availableBots.Add(bot);

        if (bot.TakenResource != null)
        {
            _acceptedTargetResources.Remove(bot.TakenResource);
            BotBroughtResource?.Invoke(bot.TakenResource);
        }

        bot.Return -= AcceptBot;
    }

    private void RemoveBot(Bot bot)
    {
        _busyBots.Remove(bot);

        bot.Return -= AcceptBot;
        bot.BaseBuilt -= RemoveBot;

        BaseBuilt?.Invoke();
    }
}
