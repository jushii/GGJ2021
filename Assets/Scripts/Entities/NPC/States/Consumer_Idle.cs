namespace Entities.NPC.States
{
    public class Consumer_Idle : State
    {
        private readonly AIManager _aiManager;
        
        public Consumer_Idle()
        {
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }
        
        public override void OnEnter(object args = null)
        {
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
            _aiManager.ChangeState(npc, typeof(Consumer_MoveToTarget));
        }
    }
}