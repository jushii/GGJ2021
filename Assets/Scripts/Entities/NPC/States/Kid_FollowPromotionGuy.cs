using UnityEngine;

public class Kid_FollowPromotionGuy : State
{
    private EntityManager _entityManager;
    private PromotionGuy _followedPromotionGuy;

    public Kid_FollowPromotionGuy()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        _entityManager.players[0].RemoveFollower(npc as Kid);
        
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