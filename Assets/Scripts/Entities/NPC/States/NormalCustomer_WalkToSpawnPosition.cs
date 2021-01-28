namespace Entities.NPC.States
{
    public class NormalCustomer_WalkToSpawnPosition : State
    {
        public override void OnEnter()
        {
            npc.MoveTo(npc.spawnPosition);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}