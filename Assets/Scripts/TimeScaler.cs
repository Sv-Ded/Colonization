using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [SerializeField] private float _scale = 1;

    private void Update()
    {
        Time.timeScale = _scale;
    }
}
