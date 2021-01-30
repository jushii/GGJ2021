public class PromotionGuy_Idle : State
{
    public override void OnEnter(object args = null)
    {
        npc.StopFollowing();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}