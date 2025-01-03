using UnityEngine;

[CreateAssetMenu(fileName = "BaseCreatorState", order = 53)]
public class BaseCreatorState:State
{
    [SerializeField] private BuildedBase _basePrefab;

    private OriginBase _base;
    private BotStation _station;

    public override void Init()
    {
        IsFinished = true;
        CristallForCreate = 2;
        SteelForCreate = 3;
    }
    
    public void SetBasePosition(Transform transform, BotStation station)
    {
        transform.TryGetComponent(out _base);
        _station = station;
    }

    public override void Run()
    {
        IsFinished = false;

        BuildedBase newBase = Instantiate(_basePrefab,_base.transform.position,Quaternion.identity);
        newBase.OnBuilded += OnBuildedBase;

        newBase.Init(_base.Flag.transform);

        _station.SendBotToBuildBase(newBase);
    }

    public void OnBuildedBase(BuildedBase buildedBase)
    {
        buildedBase.OnBuilded -= OnBuildedBase;

        _base.Flag.transform.position = _base.transform.position;
        
        IsFinished = true;
    }
}
