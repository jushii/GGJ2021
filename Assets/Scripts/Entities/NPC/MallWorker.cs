public class MallWorker : NPC
{
    public override void OnStartPunch()
    {
        aiPath.canMove = false;
        // aiManager.ChangeState(this, typeof(MallWorker_FreezeFrame));
    }

    public override void OnEndPunch()
    {
        // if (receivedHits >= 2)
        // {
        //     aiManager.ChangeState(this, typeof(MallWorker_Fly));
        // }
        // else
        // {
            aiManager.ChangeState(this, typeof(MallWorker_FreezeFrame));
        // }
    }
}