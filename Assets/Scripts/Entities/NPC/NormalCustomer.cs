public class NormalCustomer : NPC
{
    public override void OnPunch()
    {
       aiManager.ChangeState(this, typeof(NormalCustomer_Fly));
    }
}