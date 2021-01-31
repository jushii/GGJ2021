using UnityEngine;

public class Kid_FollowPlayer : State
{
    private readonly AIManager _aiManager;
    private readonly PromotionGuyManager _promotionGuyManager;
    private Player _followedPlayer;
    private Transform _followTarget;
    private Collider2D[] foundExit = new Collider2D[1];
    private bool _isPromoGuySpawnedForThisKid;
    
    public Kid_FollowPlayer()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _promotionGuyManager = ServiceLocator.Current.Get<PromotionGuyManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        Kid k = npc as Kid;
        k.PlayWalkAnimation();
        
        if (args is Player player)
        {
            _followedPlayer = player;
            
            // First add the kid as a new follower.
            _followedPlayer.AddFollower(npc as Kid);

            // Promo guy count should not exceed the follower kid count.
            if (!_isPromoGuySpawnedForThisKid)
            {
                _isPromoGuySpawnedForThisKid = true;
                _promotionGuyManager.SpawnPromotionGuy();
            }
        }
        else
        {
            _aiManager.ChangeState(npc, typeof(Kid_Idle));
        }
    }

    public override void OnExit()
    {
        _followedPlayer = null;
        _followTarget = null;
    }

    public override void OnUpdate()
    {
        Transform target = _followedPlayer.GetKidFollowTarget(npc as Kid);
        
        // Switch target if necessary!
        if (_followTarget != target)
        {
            _followTarget = target;
            npc.StartFollowing(_followTarget);
        }
        
        // LookForExit();
    }

    // private void LookForExit()
    // {
    //     int layerMask = 1 << LayerMask.NameToLayer("Exit");
    //     Vector2 kidPosition = npc.transform.position;
    //     int foundExitCount = Physics2D.OverlapBoxNonAlloc(kidPosition, Vector2.one * 2.5f, 0.0f, foundExit, layerMask);
    //     if (foundExitCount > 0)
    //     {
    //         for (int i = 0; i < foundExitCount; i++)
    //         {
    //             if (foundExit[i].TryGetComponent(out Exit exit))
    //             {
    //                 Kid kid = npc as Kid;
    //                 kid.isRescued = true;
    //                 
    //                 _followedPlayer.RemoveFollower(kid);
    //                 _aiManager.ChangeState(npc, typeof(Kid_GoToExit), exit);
    //             }
    //         }
    //     }
    // }
}