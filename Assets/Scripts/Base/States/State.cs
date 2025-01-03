using UnityEngine;

public abstract class State : ScriptableObject
{
    public int CristallForCreate { get; protected set; }
    public int SteelForCreate { get; protected set; }

    public bool IsFinished { get; protected set; }

    public virtual void Init()
    {

    }

    public abstract void Run();
}
