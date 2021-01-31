using Pathfinding;
using UnityEngine;

public class PromotionGuy_RunAwayWithKid : State
{
    private readonly AIManager _aiManager;
    private EntityManager _entityManager;
    private int _calculateFleePathFrameInterval => 60 * 12;
    
    public PromotionGuy_RunAwayWithKid()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        npc.followerKid = args as Kid;
        
        _aiManager.ChangeState(npc.followerKid, typeof(Kid_FollowPromotionGuy), npc);
        
        CalculateFleePath();
    }

    public override void OnExit()
    {
        // npc.aiPath.maxSpeed = 3;
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
        
        FleePath fleePath = FleePath.Construct(npc.transform.position, player.transform.position, 1000 * 100);
        npc.aiPath.SetPath(fleePath);
        npc.aiPath.canMove = true;
        // npc.aiPath.maxSpeed = 3;
    }
}