using UnityEngine;

public class MallWorker_Idle : State
{
    private readonly AIManager _aiManager;
    private Collider2D[] foundPlayer = new Collider2D[2];

    private float _standUpTimer;
    private float _standUpTime = 1f;
    
    public MallWorker_Idle()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        _standUpTimer = _standUpTime;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        _standUpTimer -= Time.deltaTime;
        if (_standUpTimer <= 0)
        {
            npc.stunned = false;
            LookForPlayer();
        }
    }

    private void LookForPlayer()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Vector2 mallWorkerPosition = npc.transform.position;
        int foundPlayerCount = Physics2D.OverlapBoxNonAlloc(mallWorkerPosition, Vector2.one * 16.0f, 0.0f, foundPlayer, layerMask);
        if (foundPlayerCount > 0)
        {
            for (int i = 0; i < foundPlayerCount; i++)
            {
                if (foundPlayer[i].TryGetComponent(out Player player))
                {
                    _aiManager.ChangeState(npc, typeof(MallWorker_FollowPlayer), player);
                }
            }
        }
    }
}
