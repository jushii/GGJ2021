using UnityEngine;

public class PromotionGuy_FreezeFrame : State
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