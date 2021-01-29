using UnityEngine;

namespace Entities.NPC.States
{
    public class Kid_GoToExit : State
    {
        private readonly AIManager _aiManager;
        
        public Kid_GoToExit()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
            Exit exit = args as Exit;
            npc.StopFollowing();
            npc.MoveTo(exit.transform.position);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}