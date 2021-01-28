namespace Entities.NPC.States
{
    public class Consumer_MoveToTarget : State
    {
        private readonly EntityManager _entityManager;

        public Consumer_MoveToTarget()
        {
            _entityManager = ServiceLocator.Current.Get<EntityManager>();
        }
        
        public override void OnEnter()
        {
            // npc.StartFollowing(_entityManager.players[0].transform);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}