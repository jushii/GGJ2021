using UnityEngine;

public class NormalCustomer_Idle : State
{
    private readonly AIManager _aiManager;
    private float _walkBackToOriginalPositionDst = 0.25f;
    private float _standUpTimer;
    private float _standUpTime = 1f;

    public NormalCustomer_Idle()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        float dst = Vector2.Distance(npc.transform.position, npc.spawnPosition);
        if (dst > _walkBackToOriginalPositionDst)
        {
            _standUpTimer = _standUpTime;
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        _standUpTimer -= Time.deltaTime;
        if(_standUpTimer <= 0)
        {
            npc.stunned = false;
            float dst = Vector2.Distance(npc.transform.position, npc.spawnPosition);
            if (dst > _walkBackToOriginalPositionDst)
            {
                _aiManager.ChangeState(npc, typeof(NormalCustomer_WalkToSpawnPosition));
            }
        }
    }
}
