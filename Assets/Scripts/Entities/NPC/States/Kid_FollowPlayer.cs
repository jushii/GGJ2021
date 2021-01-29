using UnityEngine;

namespace Entities.NPC.States
{
    public class Kid_FollowPlayer : State
    {
        private readonly AIManager _aiManager;
        private Player _followedPlayer;
        private Transform _followTarget;

        public Kid_FollowPlayer()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
            if (args is Player player)
            {
                _followedPlayer = player;
                
                // First add the kid as a new follower.
                _followedPlayer.AddFollower(npc as Kid);
            }
            else
            {
                _aiManager.ChangeState(npc, typeof(Kid_Idle));
            }
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            Transform transform = _followedPlayer.GetKidFollowTarget(npc as Kid);
            
            // Switch target if necessary!
            if (_followTarget != transform)
            {
                _followTarget = transform;
                npc.StartFollowing(_followTarget);
            }
        }
    }
}