public class MallWorker : NPC
{
    public override void OnStartPunch()
    {
        aiManager.ChangeState(this, typeof(MallWorker_FreezeFrame));
    }

    public override void OnEndPunch()
    {
        aiManager.ChangeState(this, typeof(MallWorker_Fly));
    }
}