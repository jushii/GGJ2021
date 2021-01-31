using UnityEngine;

public class NormalCustomer_FreezeFrame : State
{
    public override void OnEnter(object args = null)
    {
        npc.receivedHits++;
        ServiceLocator.Current.Get<AudioManager>().PlayHitSFX();
        npc.aiPath.canMove = false;
    }

    public override void OnExit()
    {
        npc.aiPath.canMove = true;
    }

    public override void OnUpdate()
    {
    }
}