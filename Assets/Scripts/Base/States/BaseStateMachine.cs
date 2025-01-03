using System;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField] private BotCreatorState _botCreatorPrefab;
    [SerializeField] private BaseCreatorState _baseCreatorPrefab;

    private State _currentState;

    public BotCreatorState BotCreatorState { get; private set; }
    public BaseCreatorState BaseCreatorState { get; private set; }

    public event Action<int, int> ResourcesUsed;

    public void Init(BotStation botStation)
    {
        BotCreatorState = Instantiate(_botCreatorPrefab);
        BotCreatorState.Init();
        BotCreatorState.AddStation(botStation);

        BaseCreatorState = Instantiate(_baseCreatorPrefab);
        BaseCreatorState.Init();
        BaseCreatorState.SetBasePosition(transform, botStation);

        _currentState = BotCreatorState;
    }

    public void Run(int cristallCount, int steelCount)
    {
        if (_currentState.CristallForCreate <= cristallCount && _currentState.SteelForCreate <= steelCount)
        {
            _currentState.Run();
            ResourcesUsed?.Invoke(_currentState.CristallForCreate, _currentState.SteelForCreate);

            if (_currentState == BaseCreatorState)
            {
                _currentState = BotCreatorState;
            }
        }
    }

    public void SetBaseCreatorState()
    {
        if (BaseCreatorState.IsFinished)
        {
            _currentState = BaseCreatorState;
        }
    }
}
