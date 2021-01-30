public class NormalCustomer : NPC
{
    public override void OnStartPunch()
    {
       aiManager.ChangeState(this, typeof(NormalCustomer_FreezeFrame));
    }
}