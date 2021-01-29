using UnityEngine;

namespace Entities.NPC.States
{
    public class MallWorker_FollowPlayer : State
    {
        private readonly AIManager _aiManager;
        private Player _followedPlayer;
        private Transform _followTarget;
        // private Collider2D[] foundExit = new Collider2D[1];

        public MallWorker_FollowPlayer()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
            if (args is Player player)
            {
                _followedPlayer = player;
                
                npc.StartFollowing(_followedPlayer.transform);
                // First add the kid as a new follower.
                // _followedPlayer.AddFollower(npc);
            }
            else
            {
                _aiManager.ChangeState(npc, typeof(MallWorker_Idle));
            }
        }

        public override void OnExit()
        {
            // _followedPlayer = null;
            // _followTarget = null;
        }

        public override void OnUpdate()
        {
            // Transform target = _followedPlayer.GetKidFollowTarget(npc);
            //
            // // Switch target if necessary!
            // if (_followTarget != target)
            // {
            //     _followTarget = target;
            //     npc.StartFollowing(_followTarget);
            // }
            //
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
        //                 _followedPlayer.RemoveFollower(npc);
        //                 _aiManager.ChangeState(npc, typeof(Kid_GoToExit), exit);
        //             }
        //         }
        //     }
        // }
    }
}