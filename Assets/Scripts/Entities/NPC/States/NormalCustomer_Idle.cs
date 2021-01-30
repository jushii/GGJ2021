using UnityEngine;

namespace Entities.NPC.States
{
    public class NormalCustomer_Idle : State
    {
        private float _walkBackToOriginalPositionDst = 0.25f;
        private float _standUpTimer;
        private float _standUpTime = 1f;
        private readonly AIManager _aiManager;

        public NormalCustomer_Idle()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
            _standUpTimer = _standUpTime;
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            _standUpTimer -= Time.deltaTime;
            if(_standUpTimer <= 0)
            {
                ((NormalCustomer)npc).stunned = false;
                float dst = Vector2.Distance(npc.transform.position, npc.spawnPosition);
                if (dst > _walkBackToOriginalPositionDst)
                {
                    _aiManager.ChangeState(npc, typeof(NormalCustomer_WalkToSpawnPosition));
                }
            }
        }
    }
}