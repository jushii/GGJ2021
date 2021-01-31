using DG.Tweening;
using UnityEngine;

public class PromotionGuy_FreezeFrame : State
{
    private int knockbackTimer;
    private int knockbackInitiationTimeInFrames = 2;
    private bool isKnockbackApplied = false;
    private float flySpeed = 10f;
    private float flyHeight = 10f;
    private float appliedPunchForce;
    private float exitStateTimer = 0;
    private float exitStateTime = 3;

    private float _hitAngle;
    private float _angleDivider = 90.0f;
    private HitDirection _hitDirection;

    public override void OnEnter(object args = null)
    {
        isKnockbackApplied = false;
        npc.receivedHits++;
        appliedPunchForce = npc.receivedHits * 5;
        switch (npc.receivedHits)
        {
            case 1: appliedPunchForce = 1;
                break;
            case 2: appliedPunchForce = 1;
                break;
            default: appliedPunchForce = 4;
                break;
        }
        
        Debug.Log("NPC RECEIVED HITS: " + npc.receivedHits);
        
        ServiceLocator.Current.Get<AudioManager>().PlayHitSFX();
        npc.aiPath.canMove = false;
        knockbackTimer = Time.frameCount;
        exitStateTimer = 0;
    }

    public override void OnExit()
    {
        npc.aiPath.canMove = true;
    }

    public override void OnUpdate()
    {
        if (!isKnockbackApplied)
        {
            if (Time.frameCount % knockbackInitiationTimeInFrames == 0)
            {
                isKnockbackApplied = true;
                var knockBackDir = (npc.transform.position - ServiceLocator.Current.Get<EntityManager>().players[0].transform.position).normalized;
                var flyNormalDirection = new Vector3(knockBackDir.y, -knockBackDir.x, 0);           // normal vector of flying direction vector                
                npc.rigidbody2D.AddForce(knockBackDir * (appliedPunchForce * flySpeed) + flyNormalDirection * flyHeight / 3, ForceMode2D.Impulse);
                
                if (knockBackDir != Vector3.zero)
                {
                    _hitAngle = Mathf.Acos(Vector2.Dot(Vector2.right, knockBackDir) / knockBackDir.magnitude);
                    if (knockBackDir.y < 0)
                    {
                        _hitAngle = 2 * Mathf.PI - _hitAngle;
                    }

                    _hitDirection = (HitDirection)Mathf.RoundToInt(_hitAngle * Mathf.Rad2Deg / _angleDivider);
                }
                else
                {
                    _hitDirection = HitDirection.None;
                }

                switch (_hitDirection)
                {
                    case HitDirection.Top:
                        npc.spriteRenderer.sprite = npc.topHitSprites[npc.HitSpriteIndexTop];
                        break;
                    case HitDirection.Bottom:
                        npc.spriteRenderer.sprite = npc.bottomHitSprites[npc.HitSpriteIndexBottom];
                        break;
                    case HitDirection.Left:
                        npc.spriteRenderer.sprite = npc.leftHitSprites[npc.HitSpriteIndexLeft];
                        break;
                    case HitDirection.Right:
                        npc.spriteRenderer.sprite = npc.rightHitSprites[npc.HitSpriteIndexRight];
                        break;
                    case HitDirection.None:
                        npc.spriteRenderer.sprite = npc.idleSprite;
                        break;
                }

            }
        }
        
        exitStateTimer += (1.0f * Time.deltaTime);
        if (exitStateTimer > exitStateTime)
        {
            npc.spriteRenderer.sprite = npc.idleSprite;
            ServiceLocator.Current.Get<AIManager>().ChangeState(npc, typeof(PromotionGuy_ChaseKid));
        }
    }
}