using Unity.VisualScripting;
using UnityEngine;

[RequireComponent  (typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 0.2f;

    private Camera _camera;
    private Vector3 _startMousePosition;
    private Vector3 _endMousePosition;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        _endMousePosition = Input.mousePosition;
        _startMousePosition = _endMousePosition;
    }

    private void FixedUpdate()
    {
        _startMousePosition = _endMousePosition;
        _endMousePosition = Input.mousePosition;

        transform.position += GetDirection()*_mouseSensitivity;
    }

    private Vector3 GetDirection()
    {
        Vector3 mouseDirection = (_endMousePosition-_startMousePosition);
        Vector3 direction = new Vector3(mouseDirection.x,Input.mouseScrollDelta.y*100,mouseDirection.y);

        return direction;
    }
}
