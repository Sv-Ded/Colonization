using UnityEngine;

public class MaterialChanger:MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _materialBase;
    [SerializeField] private Material _materialSelection;

    private Color _baseColor;
    private Color _selectionColor;

    public void Init()
    {
        _baseColor = _materialBase.color;
        _selectionColor = _materialSelection.color;
    }

    public void SetBaseColor() => _renderer.materials[0].SetColor("_Color", _baseColor);

    public void SetSelectionColor()=> _renderer.materials[0].SetColor("_Color",_selectionColor);
}