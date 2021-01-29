using UnityEngine;

public class Consumer_Fly : State
{
    private readonly EntityManager _entityManager;
    private readonly AIManager _aiManager;

    private Vector3 flyDirection;
    private float stillSpeed = 0.5f;
    private float flyingSpeed = 10f;

    public Consumer_Fly()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        _aiManager = ServiceLocator.Current.Get<AIManager>();
    }

    public override void OnEnter(object args = null)
    {
        npc.StopFollowing();
        npc.aiPath.rvoDensityBehavior.enabled = false; 
        flyDirection = (npc.transform.position - _entityManager.players[0].transform.position).normalized;
        npc.rigidbody2D.AddForce(flyDirection.normalized * flyingSpeed, ForceMode2D.Impulse);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (npc.rigidbody2D.velocity.magnitude < stillSpeed)
        {
            npc.rigidbody2D.velocity = Vector2.zero;
            npc.aiPath.rvoDensityBehavior.enabled = true;
            _aiManager.ChangeState(npc, typeof(Consumer_MoveToTarget));
        }
    }
}