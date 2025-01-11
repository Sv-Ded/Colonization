using TMPro;
using UnityEngine;

public class StorageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private string _resource1Name = "Кристалл";
    private string _resource2Name = "Сталь";

    private void Start()
    {
        _text.text = _resource1Name + ": 0;" + "\n" + _resource2Name + ": 0;";
    }

    public void UpdateText(int resource1,int resource2)
    {
        _text.text = _resource1Name +": " +resource1+"\n" +_resource2Name+": "+ resource2;
    }
}
