using System.Collections.Generic; 
using UnityEngine;

public class PromotionGuy : NPC
{
    public List<Sprite> speechBubbleSprites;
    public bool hurt;

    public override void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        spawnPosition = transform.position;

        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);

        stunned = false;
        hurt = false;
    }

    public override void OnStartPunch()
    {
        if (followerKid != null)
        {
            Kid kid = followerKid;
            aiManager.ChangeState(kid, typeof(Kid_FollowPlayer), ServiceLocator.Current.Get<EntityManager>().players[0]);
            followerKid = null;
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
