using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcesPool _pool;
    [SerializeField] private float _delay;
    [SerializeField] private int _minCountResources;
    [SerializeField] private int _maxCountResources;
    [SerializeField] private float _distance = 10;

    private WaitForSeconds _spawnDelay;

    private void Start()
    {
        _spawnDelay = new WaitForSeconds(_delay);

        StartCoroutine(SpawnerCoroutine());
    }

    private IEnumerator SpawnerCoroutine()
    {
        Resource resource;

        while (enabled)
        {
            int count = Random.Range(_minCountResources, _maxCountResources);

            for (int i = 0; i < count; i++)
            {
                resource = _pool.GetResource();

                resource.Init(GetSpawnPosition());
            }


            yield return _spawnDelay;
        }
    } 

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-_distance, _distance), 0, Random.Range(-_distance, _distance));
    }
}
