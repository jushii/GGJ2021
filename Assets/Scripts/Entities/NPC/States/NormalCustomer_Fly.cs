using UnityEngine;
using DG.Tweening;

namespace Entities.NPC.States
{
    public class NormalCustomer_Fly : State
    {
        private readonly EntityManager _entityManager;
        private readonly AIManager _aiManager;

        private Vector3 flyDirection;
        private float stillSpeed = 1f;
        private float flyingSpeed = 10f;
        private float flyHeight = 5f;
        private bool _flyStarted;
        private float _freezeTimer;
        // private float _freezeTime = 0.15f;
        private float _freezeTime = 0.05f;
        private float _stunnedTimer;
        private float _stunnedTime = 1.5f;

        private int flyingAnimationType;
        
        public NormalCustomer_Fly()
        {
            _entityManager = ServiceLocator.Current.Get<EntityManager>();
            _aiManager = ServiceLocator.Current.Get<AIManager>();
        }

        public override void OnEnter(object args = null)
        {
            _flyStarted = false;
            _freezeTimer = _freezeTime;
            ((NormalCustomer)npc).stunned = false;
            _stunnedTimer = _stunnedTime;
            
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
                    //npc.rigidbody2D.AddForce(flyDirection.normalized * flyingSpeed, ForceMode2D.Impulse);

                    npc.rigidbody2D.DOJump(npc.transform.position + flyDirection * flyingSpeed, flyHeight, 1, 1f);    
                }
            }

            if (_flyStarted)
            {
                if (npc.rigidbody2D.velocity.magnitude < stillSpeed)
                {
                    npc.rigidbody2D.velocity = Vector2.zero;
                    npc.aiPath.rvoDensityBehavior.enabled = true;

                    if (((NormalCustomer)npc).stunned)
                    {
                        //_stunnedTimer -= Time.deltaTime;
                        //if(_stunnedTimer <= 0)
                        //{
                        ((NormalCustomer)npc).ResetAnimatorTriggers();
                        ((NormalCustomer)npc).Animator.SetTrigger("Idle");

                        _aiManager.ChangeState(npc, typeof(NormalCustomer_Idle));
                        //}
                    }
                }
                else
                {
                    ((NormalCustomer)npc).ResetAnimatorTriggers();

                    ((NormalCustomer)npc).stunned = true;
                    flyingAnimationType = Random.Range(1, 100);
                    if (flyingAnimationType % 2 == 0)
                    {
                        ((NormalCustomer)npc).Animator.SetTrigger("Fly1");
                    }
                    else
                    {
                        ((NormalCustomer)npc).Animator.SetTrigger("Fly2");
                    }
                }
            }
        }
    }
}