using System.Collections.Generic;
using UnityEngine;

public class PromotionGuy_ChaseKid : State
{
    private readonly AIManager _aiManager;
    private EntityManager _entityManager;
    private PromotionGuyManager _promotionGuyManager;
    private Player _followedPlayer;
    private Kid _followedKid;
    private Collider2D[] foundKid = new Collider2D[1];

    public PromotionGuy_ChaseKid()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        _promotionGuyManager = ServiceLocator.Current.Get<PromotionGuyManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        List<Player> players = _entityManager.players;
        _followedPlayer = players[0];
    }

    public override void OnExit()
    {
        _followedPlayer = null;
        _followedKid = null;
    }

    public override void OnUpdate()
    {
        if (_followedKid != null && _followedKid.isRescued)
        {
            _followedKid = null;
            // _promotionGuyManager.SendPromotionGuyToPrison(npc as PromotionGuy);
            return;
        }
        
        if (DoesPlayerHaveFollowers())
        {
            npc.aiPath.canSearch = true;
            npc.aiPath.canMove = true;
            
            Kid kid = _followedPlayer.GetPromotionGuyFollowTargetKid();

            if (kid.state.GetType() == typeof(Kid_FollowPromotionGuy))
            {
                _followedPlayer.RemoveFollower(kid);
                return;
            }
            
            // Switch target if necessary!
            if (_followedKid != kid)
            {
                _followedKid = kid;
                npc.StartFollowing(_followedKid.transform);
            }

            LookForKid();
        }
    }

    private bool DoesPlayerHaveFollowers()
    {
        if (_followedPlayer.followers.Count == 0)
        {
            return false;
        }

        return true;
    }
    
    private void LookForKid()
    {
        if (_followedKid.state.GetType() == typeof(Kid_FollowPromotionGuy))
        {
            _followedKid = null;
            return;
        }
        
        int layerMask = 1 << LayerMask.NameToLayer("Kid");
        Vector2 promotionGuyPosition = npc.transform.position;
        int foundKidCount = Physics2D.OverlapBoxNonAlloc(promotionGuyPosition, Vector2.one * 2.5f, 0.0f, foundKid, layerMask);
        if (foundKidCount > 0)
        {
            for (int i = 0; i < foundKidCount; i++)
            {
                if (foundKid[i].TryGetComponent(out Kid kid))
                {
                    // If we found the kid we're following!
                    if (kid == _followedKid)
                    {
                        if (kid.state.GetType() == typeof(Kid_FollowPromotionGuy))
                        {
                            return;
                        }
                        
                        _followedPlayer.RemoveFollower(kid);
                        npc.StopFollowing();
                        _aiManager.ChangeState(npc, typeof(PromotionGuy_RunAwayWithKid), kid);
                    }
                }
            }
        }
    }
}
