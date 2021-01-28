using UnityEngine;

namespace Entities.NPC.States
{
    public class Consumer_Fly : State
    {
        private readonly EntityManager _entityManager;
        private readonly AIManager _aiManager;

        private Vector3 flyDirection;
        private float stillSpeed = 0.5f;
        private float flyingSpeed = 10f;

        public Consumer_Fly()
        {
            _entityManager = ServiceLocator.Current.Get<EntityManager>();
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }

        public override void OnEnter()
        {
            npc.StopFollowing();
            flyDirection = npc.transform.position - _entityManager.players[0].transform.position;
            npc.GetComponent<Rigidbody2D>().AddForce(flyDirection.normalized * flyingSpeed, ForceMode2D.Impulse);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            if(npc.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < stillSpeed)
            {
                _aiManager.ChangeState(npc, typeof(Consumer_MoveToTarget));
            }
        }
    }
}