public class NormalCustomer : NPC
{
    public override void OnPunch()
    {
       aiManager.ChangeState(this, typeof(Entities.NPC.States.NormalCustomer_Fly));
    }
}