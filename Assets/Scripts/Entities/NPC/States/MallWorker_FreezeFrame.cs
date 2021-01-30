using UnityEngine;

public class MallWorker_FreezeFrame : State
{
    public override void OnEnter(object args = null)
    {
        npc.aiPath.canMove = false;
        npc.rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    public override void OnExit()
    {
        npc.aiPath.canMove = true;
        npc.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void OnUpdate()
    {
    }
}