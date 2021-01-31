using UnityEngine;

public class Kid_Idle : State
{
    private readonly AIManager _aiManager;
    private Collider2D[] foundPlayer = new Collider2D[2];
    private float speechBubbleTimer = 0;
    private float speechBubbleTime;

    private int speechBubbleIndex;

    public Kid_Idle()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        speechBubbleTime = Random.Range(3, 6);
    }
    
    public override void OnEnter(object args = null)
    {
        Kid k = npc as Kid;
        k.PlayIdleAnimation();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        speechBubbleTimer += (1.0f * Time.deltaTime);
        if (speechBubbleTimer > speechBubbleTime)
        {
            // TODO: Instantiate sprite at kid position.
            GameObject speechBubbleObject = new GameObject("SpeechBubble");
            speechBubbleObject.transform.parent = npc.gameObject.transform;
            speechBubbleObject.transform.localPosition = new Vector3(0.1f, 3.23f, 0f);
            speechBubbleObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            SpriteRenderer spriteRenderer = speechBubbleObject.AddComponent<SpriteRenderer>();

            speechBubbleIndex = Random.Range(0, ((Kid)npc).speechBubbleSprites.Count - 1);
            spriteRenderer.sprite = ((Kid)npc).speechBubbleSprites[speechBubbleIndex];

            Object.Destroy(speechBubbleObject, 2);

            speechBubbleTimer = 0.0f;
        }
        
        LookForPlayer();
    }

    private void LookForPlayer()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Vector2 kidPosition = npc.transform.position;
        int foundPlayerCount = Physics2D.OverlapBoxNonAlloc(kidPosition, Vector2.one * 2.5f, 0.0f, foundPlayer, layerMask);
        if (foundPlayerCount > 0)
        {
            for (int i = 0; i < foundPlayerCount; i++)
            {
                if (foundPlayer[i].TryGetComponent(out Player player))
                {
                    _aiManager.ChangeState(npc, typeof(Kid_FollowPlayer), player);
                }
            }
        }
    }
}
