using UnityEngine;

public class Kid_FollowPromotionGuy : State
{
    private PromotionGuy _followedPromotionGuy;

    public Kid_FollowPromotionGuy()
    {
    }
    
    public override void OnEnter(object args = null)
    {
        Kid k = npc as Kid;
        k.PlayWalkAnimation();
        
        _followedPromotionGuy = args as PromotionGuy;
        npc.StartFollowing(_followedPromotionGuy.transform);
    }

    public override void OnExit()
    {
        _followedPromotionGuy = null;
        npc.StopFollowing();
    }

    public override void OnUpdate()
    {
    }
}