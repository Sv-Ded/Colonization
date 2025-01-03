using UnityEngine;

public class RecipesStorage : ScriptableObject
{
    private int _cristallForCreateBot = 2;
    private int _steelForCreateBot = 1;

    private int _cristallForCreateBase = 3;
    private int _steelForCreateBase = 2;


    public bool TryCreateBot(int cristallCount, int steelCount) => cristallCount >= _cristallForCreateBot && steelCount >= _steelForCreateBot;

    public bool TryCreateBase(int cristallCount, int steelCount) => cristallCount >= _cristallForCreateBase && steelCount >= _steelForCreateBase;
}
