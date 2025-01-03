using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BotCreatorState",order =53)]
public class BotCreatorState : State
{
    private BotStation _station;

    public override void Init()
    {
        CristallForCreate = 0;
        SteelForCreate = 3;
    }

    public void AddStation(BotStation station)
    {
        _station = station;
    }

    public override void Run()
    {
        _station.AddBot();
    }
}
