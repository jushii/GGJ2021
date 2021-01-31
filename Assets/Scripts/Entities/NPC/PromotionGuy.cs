
public class PromotionGuy : NPC
{
    public override void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        spawnPosition = transform.position;

        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);

        stunned = false;
    }

    public override void OnStartPunch()
    {
        if (state.GetType() == typeof(PromotionGuy_RunAwayWithKid))
        {
            PromotionGuy_RunAwayWithKid runAwayState = state as PromotionGuy_RunAwayWithKid;
            Kid kid = runAwayState.followerKid;
            aiManager.ChangeState(kid, typeof(Kid_FollowPlayer), ServiceLocator.Current.Get<EntityManager>().players[0]);
        }

        aiPath.canMove = false;
        // aiManager.ChangeState(this, typeof(PromotionGuy_FreezeFrame));
    }

    public override void OnEndPunch()
    {
        // if (receivedHits >= 2)
        // {
        //     aiManager.ChangeState(this, typeof(PromotionGuy_Fly));
        // }
        // else
        // {
            aiManager.ChangeState(this, typeof(PromotionGuy_FreezeFrame));
        // }
    }
}
