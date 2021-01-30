using UnityEngine;

public class MallWorker_Fly : State
{
    private readonly EntityManager _entityManager;
    private readonly AIManager _aiManager;

    private Vector3 flyDirection;
    private float stillSpeed = 0.5f;
    private float flyingSpeed = 25f;
    private bool _flyStarted;
    private float _freezeTimer;
    // private float _freezeTime = 0.15f;
    private float _freezeTime = 0.05f;
    
    public MallWorker_Fly()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        _aiManager = ServiceLocator.Current.Get<AIManager>();
    }

    public override void OnEnter(object args = null)
    {
        _flyStarted = false;
        _freezeTimer = _freezeTime;
        
        npc.StopFollowing();
        npc.aiPath.rvoDensityBehavior.enabled = false;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (!_flyStarted)
        {
            _freezeTimer -= Time.deltaTime;
            if (_freezeTimer <= 0)
            {
                _flyStarted = true;

                flyDirection = (npc.transform.position - _entityManager.players[0].transform.position).normalized;
                npc.rigidbody2D.AddForce(flyDirection.normalized * flyingSpeed, ForceMode2D.Impulse);
            }
        }
        
        if (_flyStarted && npc.rigidbody2D.velocity.magnitude < stillSpeed)
        {
            npc.rigidbody2D.velocity = Vector2.zero;
            npc.aiPath.rvoDensityBehavior.enabled = true;
            _aiManager.ChangeState(npc, typeof(MallWorker_Idle));
        }
    }
}