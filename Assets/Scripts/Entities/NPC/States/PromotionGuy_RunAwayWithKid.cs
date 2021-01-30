using Pathfinding;
using UnityEngine;

public class PromotionGuy_RunAwayWithKid : State
{
    private readonly AIManager _aiManager;
    private EntityManager _entityManager;
    private int _calculateFleePathFrameInterval => 60 * 6;
    public Kid followerKid;
    
    public PromotionGuy_RunAwayWithKid()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        followerKid = args as Kid;
        
        _aiManager.ChangeState(followerKid, typeof(Kid_FollowPromotionGuy), npc);
        
        CalculateFleePath();
    }

    public override void OnExit()
    {
        followerKid = null;
        npc.ClearPath();
    }

    public override void OnUpdate()
    {
        CalculateFleePathOnInterval();
    }

    private void CalculateFleePathOnInterval()
    {
        if (Time.frameCount % _calculateFleePathFrameInterval == 0)
        {
            CalculateFleePath();
        }
    }

    private void CalculateFleePath()
    {
        Player player = _entityManager.players[0];

        npc.StopFollowing();
        npc.aiPath.canSearch = false;
        
        FleePath fleePath = FleePath.Construct(npc.transform.position, player.transform.position, 1000 * 50);
        npc.aiPath.SetPath(fleePath);
        npc.aiPath.canMove = true;
    }
}