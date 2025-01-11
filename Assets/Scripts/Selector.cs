using UnityEngine;

[RequireComponent(typeof(CameraMover), typeof(Camera))]
public class Selector : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;

    private CameraMover _mover;
    private Camera _camera;

    [SerializeField] private OriginBase _selectedBase;

    private void Awake()
    {
        _mover = GetComponent<CameraMover>();
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        _mover.LMBClicked += Select;

        _mover.RMBClicked += ResetSelection;
    }

    private void Select()
    {
        RaycastHit hit;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out OriginBase originBase) && _selectedBase != originBase)
            {
                _selectedBase = originBase;
                _selectedBase.SetSelectionColor();
            }
            else if (hit.collider.TryGetComponent<Ground>(out _)&& _selectedBase!=null)
            {
                _selectedBase.SetFlagPosition(hit.point);
            }
        }
    }

    private void ResetSelection()
    {
        if (_selectedBase != null)
        {
            _selectedBase.SetBaseColor();
            _selectedBase = null;
        }
    }

    private void OnDisable()
    {
        _mover.LMBClicked -= Select;

        _mover.RMBClicked -= ResetSelection;
    }
}
