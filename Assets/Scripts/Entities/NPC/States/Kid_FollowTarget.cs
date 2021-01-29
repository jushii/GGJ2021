using System.Collections.Generic;
using UnityEngine;

namespace Entities.NPC.States
{
    public class Kid_FollowTarget : State
    {
        private readonly EntityManager _entityManager;
        private readonly AIManager _aiManager;
        // private bool _isFollowing;
        private Transform _followTarget;
        
        public Kid_FollowTarget()
        {
            _entityManager = ServiceLocator.Current.Get<EntityManager>();
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
            if (args is Transform t)
            {
                _followTarget = t;
            }
            else
            {
                _aiManager.ChangeState(npc, typeof(Kid_Idle));
            }
            
            npc.StartFollowing(_followTarget);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            // if (!_isFollowing)
            // {
            //     List<Player> players = _entityManager.players;
            //     if (players.Count > 0)
            //     {
            //         _isFollowing = true;
            //         npc.StartFollowing(_entityManager.players[0].transform);
            //     }
            // }
        }
    }
}