using System;
using UnityEngine;

[RequireComponent  (typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 0.2f;

    private float _mouseScrollMultipli = 100;
    private Vector3 _startMousePosition;
    private Vector3 _endMousePosition;

    public event Action OnLMBClick;
    public event Action OnRMBClick;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        _endMousePosition = Input.mousePosition;
        _startMousePosition = _endMousePosition;
    }

    private void Update()
    {
        _startMousePosition = _endMousePosition;
        _endMousePosition = Input.mousePosition;

        transform.position += GetDirection()*_mouseSensitivity;

        if (Input.GetMouseButtonDown(0))
        {
            OnLMBClick?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRMBClick?.Invoke();
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 mouseDirection = (_endMousePosition-_startMousePosition);
        Vector3 direction = new Vector3(mouseDirection.x,Input.mouseScrollDelta.y*_mouseScrollMultipli,mouseDirection.y);

        return direction;
    }
}
