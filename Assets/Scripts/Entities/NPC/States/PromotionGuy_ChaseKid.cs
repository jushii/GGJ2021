using System.Collections.Generic;
using UnityEngine;

public class PromotionGuy_ChaseKid : State
{
    private readonly AIManager _aiManager;
    private EntityManager _entityManager;
    private PromotionGuyManager _promotionGuyManager;
    private Player _followedPlayer;
    private Kid _followedKid;
    private Collider2D[] foundKid = new Collider2D[1];

    private float speechBubbleTimer = 0;
    private float speechBubbleTime;
    private int speechBubbleIndex;

    public PromotionGuy_ChaseKid()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        _promotionGuyManager = ServiceLocator.Current.Get<PromotionGuyManager>();

        speechBubbleTime = Random.Range(3, 6);
    }
    
    public override void OnEnter(object args = null)
    {
        List<Player> players = _entityManager.players;
        _followedPlayer = players[0];
    }

    public override void OnExit()
    {
        _followedPlayer = null;
        _followedKid = null;
    }

    public override void OnUpdate()
    {
        // TODO: SPEECH BUBBLE!!!!!
        speechBubbleTimer += (1.0f * Time.deltaTime);
        if (speechBubbleTimer > speechBubbleTime)
        {
            GameObject speechBubbleObject = new GameObject("SpeechBubble");
            speechBubbleObject.transform.parent = npc.gameObject.transform;
            speechBubbleObject.transform.localPosition = new Vector3(0.2f, 4f, 0f);
            speechBubbleObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            SpriteRenderer spriteRenderer = speechBubbleObject.AddComponent<SpriteRenderer>();

            speechBubbleIndex = Random.Range(0, ((PromotionGuy)npc).speechBubbleSprites.Count - 1);
            spriteRenderer.sortingOrder = 15;
            spriteRenderer.sprite = ((PromotionGuy)npc).speechBubbleSprites[speechBubbleIndex];

            if (((PromotionGuy)npc).hurt)
            {
                Object.Destroy(speechBubbleObject);
            }
            else
            {
                Object.Destroy(speechBubbleObject, 2);
            }

            speechBubbleTimer = 0.0f;
        }

        if (_followedKid != null && _followedKid.isRescued)
        {
            _followedKid = null;
            _promotionGuyManager.SendPromotionGuyToPrison(npc as PromotionGuy);
            return;
        }
        
        if (DoesPlayerHaveFollowers())
        {
            npc.aiPath.canSearch = true;
            npc.aiPath.canMove = true;
            
            Kid kid = _followedPlayer.GetPromotionGuyFollowTargetKid();

            // Switch target if necessary!
            if (_followedKid != kid)
            {
                _followedKid = kid;
                npc.StartFollowing(_followedKid.transform);
            }

            LookForKid();
        }
    }

    private bool DoesPlayerHaveFollowers()
    {
        if (_followedPlayer.followers.Count == 0)
        {
            return false;
        }

        return true;
    }
    
    private void LookForKid()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Kid");
        Vector2 promotionGuyPosition = npc.transform.position;
        int foundKidCount = Physics2D.OverlapBoxNonAlloc(promotionGuyPosition, Vector2.one * 2.5f, 0.0f, foundKid, layerMask);
        if (foundKidCount > 0)
        {
            for (int i = 0; i < foundKidCount; i++)
            {
                if (foundKid[i].TryGetComponent(out Kid kid))
                {
                    // If we found the kid we're following!
                    if (kid == _followedKid)
                    {
                        _followedPlayer.RemoveFollower(kid);
                        npc.StopFollowing();
                        _aiManager.ChangeState(npc, typeof(PromotionGuy_RunAwayWithKid), kid);
                    }
                }
            }
        }
    }
}
