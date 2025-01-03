using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(BotSpawner))]
public class BotStation : MonoBehaviour
{
    private BotSpawner _spawner;
    private List<Bot> _availableBots;
    private List<Bot> _busyBots;
    private List<Resource> _acceptedTargetResources;
    private Coroutine _coroutine;

    public int BotCount { get { return _availableBots.Count + _busyBots.Count; } }

    public event Action<Resource> BotBroughtResource;

    public void Init(int botCount)
    {
        _spawner = GetComponent<BotSpawner>();
        _availableBots = new List<Bot>();
        _busyBots = new List<Bot>();
        _acceptedTargetResources = new List<Resource>();

        for (int i = 0; i < botCount; i++)
        {
            AddBot();
        }
    }

    public void AddBot()
    {
        Bot bot = _spawner.CreateBot();
        bot.Init(transform);
        bot.BotJoinedNewBase += RemoveBot;

        _availableBots.Add(bot);
    }

    public void AddBot(Bot bot)
    {
        bot.Init(transform);
        _availableBots.Add(bot);

        bot.BotJoinedNewBase += RemoveBot;
    }

    public void AcceptResources(Queue<Resource> resources)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SendBotToResources(resources));
    }

    public void SendBotToBuildBase(BuildedBase newBase)
    {
        StartCoroutine(WaitAvailableBot(newBase.transform));
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
                    SetBotStatus(resource.transform);
                }
            }

            yield return null;
        }
    }

    private IEnumerator WaitAvailableBot(Transform transform)
    {
        while (_availableBots.Count == 0)
        {
            yield return null;
        }

        SetBotStatus(transform);
    }

    private void SetBotStatus(Transform target)
    {
        Bot bot = _availableBots[0];

        _availableBots.Remove(bot);

        _busyBots.Add(bot);

        bot.WalkToTarget(target);

        bot.BotReturn += AcceptBot;
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

        bot.BotReturn -= AcceptBot;
    }

    private void RemoveBot(Bot bot)
    {
        _busyBots.Remove(bot);

        bot.BotReturn -= AcceptBot;
        bot.BotJoinedNewBase -= RemoveBot;
    }
}
