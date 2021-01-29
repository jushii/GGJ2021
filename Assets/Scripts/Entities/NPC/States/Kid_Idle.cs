using UnityEngine;

namespace Entities.NPC.States
{
    public class Kid_Idle : State
    {
        private readonly AIManager _aiManager;

        public Kid_Idle()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}