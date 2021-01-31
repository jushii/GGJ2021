public class NormalCustomer : NPC
{
    public override void OnStartPunch()
    {
       aiManager.ChangeState(this, typeof(NormalCustomer_FreezeFrame));
    }

    public override void OnEndPunch()
    {
        aiManager.ChangeState(this, typeof(NormalCustomer_Fly));
    }
}