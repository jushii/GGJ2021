using System;

public struct Transition
{
    public NPC npc;
    public Type nextState;
    public object onEnterArgs;
}

public abstract class State
{
    public NPC npc;
    public abstract void OnEnter(object args = null);
    public abstract void OnExit();
    public abstract void OnUpdate();
}