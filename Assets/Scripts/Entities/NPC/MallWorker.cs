public class MallWorker : NPC
{
    public override void OnPunch()
    {
        aiManager.ChangeState(this, typeof(Entities.NPC.States.MallWorker_Fly));
    }
}