using System.Collections.Generic;

namespace Entities.NPC.States
{
    public class Consumer_MoveToTarget : State
    {
        private readonly EntityManager _entityManager;
        private bool _isFollowing;
        
        public Consumer_MoveToTarget()
        {
            _entityManager = ServiceLocator.Current.Get<EntityManager>();
        }
        
        public override void OnEnter()
        {
            _isFollowing = false;
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            if (!_isFollowing)
            {
                List<Player> players = _entityManager.players;
                if (players.Count > 0)
                {
                    _isFollowing = true;
                    npc.StartFollowing(_entityManager.players[0].transform);
                }
            }
        }
    }
}