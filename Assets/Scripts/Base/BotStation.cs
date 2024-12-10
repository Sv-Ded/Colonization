using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BotStation : MonoBehaviour
{
    [SerializeField] private int _botCount;
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private Transform _botPosition;

    private List<Bot> _availableBots;
    private List<Bot> _busyBots;

    public event Action<Resource> BotBroughtResource;

    public void Init()
    {
        _availableBots = new List<Bot>();
        _busyBots = new List<Bot>();

        for (int i = 0; i < _botCount; i++)
        {
            Bot bot = Instantiate(_botPrefab, _botPosition.position, Quaternion.identity);

            bot.Init(transform);

            _availableBots.Add(bot);
        }
    }

    public void SendBotToResources(Queue<Resource> resources)
    {
        Resource resource;

        while (resources.Count > 0)
        {
            if (_availableBots.Count > 0)
            {
                resource = resources.Dequeue();

                SetBotStatus(resource);
            }
        }
    }

    private void SetBotStatus(Resource resource)
    {
        Bot bot = _availableBots[0];

        _availableBots.Remove(bot);

        _busyBots.Add(bot);

        bot.WalkToTarget(resource.transform);

        bot.BotReturn += AcceptBot;
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
