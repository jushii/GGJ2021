using UnityEngine;

namespace Entities.NPC.States
{
    public class NormalCustomer_Idle : State
    {
        private float _walkBackToOriginalPositionDst = 0.25f;
        private readonly AIManager _aiManager;

        public NormalCustomer_Idle()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
            float dst = Vector2.Distance(npc.transform.position, npc.spawnPosition);
            if (dst > _walkBackToOriginalPositionDst)
            {
                _aiManager.ChangeState(npc, typeof(NormalCustomer_WalkToSpawnPosition));
            }
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}