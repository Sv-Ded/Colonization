using UnityEngine;

[CreateAssetMenu(fileName = "BaseCreatorState", order = 53)]
public class BaseCreatorState:State
{
    [SerializeField] private BuiltBase _basePrefab;

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

        _station.BaseBuilt += OnBuildedBase;
        _station.SendBotToBuildBase(_base.Flag.transform);
    }

    public void OnBuildedBase()
    {
        _station.BaseBuilt -= OnBuildedBase;

        _base.Flag.transform.position = _base.transform.position;
        
        IsFinished = true;
    }
}
