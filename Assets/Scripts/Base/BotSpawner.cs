using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public List<Bot> CreateBot(int count)
    {
        List<Bot> botList = new List<Bot>();

        for (int i = 0; i < count; i++)
        {
            Bot bot = Instantiate(_botPrefab,transform.position,Quaternion.identity);

            bot.Init(transform);

            botList.Add(bot);
        }

        return botList;
    }
}
