using System;

public struct Transition
{
    public NPC npc;
    public Type nextState;
}

public abstract class State
{
    public NPC npc;
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}