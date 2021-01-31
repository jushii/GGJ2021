using UnityEngine;

public class Kid_Idle : State
{
    private readonly AIManager _aiManager;
    private Collider2D[] foundPlayer = new Collider2D[2];
    public Kid_Idle()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
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
