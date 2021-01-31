public class NormalCustomer : NPC
{
    public override void OnStartPunch()
    {
       // aiManager.ChangeState(this, typeof(NormalCustomer_FreezeFrame));
       aiPath.canMove = false;
    }

    public override void OnEndPunch()
    {
        // if (receivedHits >= 2)
        // {
        //     aiManager.ChangeState(this, typeof(NormalCustomer_Fly));
        // }
        // else
        // {
            aiManager.ChangeState(this, typeof(NormalCustomer_FreezeFrame));
        // }
    }
}