using UnityEngine;

public class MallWorker_Fly : State
{
    private readonly EntityManager _entityManager;
    private readonly AIManager _aiManager;

    private Vector3 flyDirection;
    private Vector3 flyNormalDirection;

    private float flyingSpeed = 25f;
    private float flyHeightCounter;
    private float flyHeight = 15f;
    private bool _flyStarted;

    private float _flyTimer;
    private float _flyTime = 1f;

    private float _freezeTimer;
    private float _freezeTime = 0.05f;
    private float _timeMultiplier = 5f;

    private int flyingAnimationType;
    public MallWorker_Fly()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        _aiManager = ServiceLocator.Current.Get<AIManager>();
    }

    public override void OnEnter(object args = null)
    {
        _flyStarted = false;
        _freezeTimer = _freezeTime;
        _flyTimer = _flyTime;

        npc.stunned = false;
        flyHeightCounter = flyHeight;

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
                flyNormalDirection = new Vector3(flyDirection.y, -flyDirection.x, 0);           // normal vector of flying direction vector                
                npc.rigidbody2D.AddForce(flyDirection * flyingSpeed + flyNormalDirection * flyHeight / 3, ForceMode2D.Impulse);
                // ServiceLocator.Current.Get<AudioManager>().PlayHitSFX();
            }
        }

        if (_flyStarted)
        {
            _flyTimer -= Time.deltaTime;
            if (_flyTimer <= 0)
            {
                npc.rigidbody2D.velocity = Vector2.zero;
                npc.aiPath.rvoDensityBehavior.enabled = true;

                if (npc.stunned)
                {
                    npc.ResetAnimatorTriggers();
                    npc.Animator.SetTrigger("Idle");

                    _aiManager.ChangeState(npc, typeof(MallWorker_Idle));
                }
            }
            else
            {
                npc.rigidbody2D.AddForce(-flyNormalDirection * flyHeightCounter, ForceMode2D.Force);
                flyHeightCounter -= _timeMultiplier * Time.deltaTime;
                if (flyHeightCounter <= 0)
                {
                    flyHeightCounter = 0;
                }

                npc.ResetAnimatorTriggers();
                npc.stunned = true;

                // randomly choose to play flying animation
                flyingAnimationType = Random.Range(1, 100);
                if (flyingAnimationType % 2 == 0)
                {
                    npc.Animator.SetTrigger("Fly1");
                }
                else
                {
                    npc.Animator.SetTrigger("Fly2");
                }
            }
        }
    }

}

