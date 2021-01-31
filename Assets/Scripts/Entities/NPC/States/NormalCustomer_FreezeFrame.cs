using DG.Tweening;
using UnityEngine;

public class NormalCustomer_FreezeFrame : State
{
    private int knockbackTimer;
    private int knockbackInitiationTimeInFrames = 2;
    private bool isKnockbackApplied = false;
    private float flySpeed = 10f;
    private float flyHeight = 10f;
    private float appliedPunchForce;
    private float exitStateTimer = 0;
    private float exitStateTime = 3;
    
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
                
                // EXAMPLE!
                // npc.spriteRenderer.sprite = npc.topHitSprites[npc.HitSpriteIndexTop];
            }
        }
        
        exitStateTimer += (1.0f * Time.deltaTime);
        if (exitStateTimer > exitStateTime)
        {
            // TODO: Change sprite back to idle sprite!
            ServiceLocator.Current.Get<AIManager>().ChangeState(npc, typeof(NormalCustomer_Idle));
        }
    }
}