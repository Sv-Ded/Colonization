using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcesPool _steelPool;
    [SerializeField] private ResourcesPool _cristallPool;
    [SerializeField] private float _delay;
    [SerializeField] private int _minCountResources;
    [SerializeField] private int _maxCountResources;
    [SerializeField] private float _distance = 10;

    private WaitForSeconds _spawnDelay;

    private void Start()
    {
        _spawnDelay = new WaitForSeconds(_delay);

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        Resource resource;

        while (enabled)
        {
            int count = Random.Range(_minCountResources, _maxCountResources);

            for (int i = 0; i < count; i++)
            {
                resource = _steelPool.GetResource();

                resource.Init(GetSpawnPosition());

                if (Random.value >= 0.4f)
                {
                    resource = _cristallPool.GetResource();

                    resource.Init(GetSpawnPosition());
                }
            }

            yield return _spawnDelay;
        }
    } 

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-_distance, _distance), 0, Random.Range(-_distance, _distance));
    }
}
