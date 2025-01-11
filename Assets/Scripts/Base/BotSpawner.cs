using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot CreateBot()=> Instantiate(_botPrefab, transform.position, Quaternion.identity);
}
