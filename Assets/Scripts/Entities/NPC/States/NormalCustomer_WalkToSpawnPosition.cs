public class NormalCustomer_WalkToSpawnPosition : State
{
    public override void OnEnter(object args = null)
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
