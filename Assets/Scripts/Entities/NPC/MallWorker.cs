public class MallWorker : NPC
{
    public override void OnPunch()
    {
        aiManager.ChangeState(this, typeof(MallWorker_Fly));
    }
}