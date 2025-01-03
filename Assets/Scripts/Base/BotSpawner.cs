using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot CreateBot()
    {
        Bot bot = Instantiate(_botPrefab, transform.position, Quaternion.identity);

        return bot;
    }
}
