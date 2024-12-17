using UnityEngine;

public class TimeSkaler : MonoBehaviour
{
    [SerializeField] private float _skale = 1;

    private void Update()
    {
        Time.timeScale = _skale;
    }
}
