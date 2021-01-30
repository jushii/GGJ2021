using DG.Tweening;
using UnityEngine;

public class Kid_GoToExit : State
{
    private readonly AIManager _aiManager;
    private readonly PromotionGuyManager _promotionGuyManager;
    
    public Kid_GoToExit()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _promotionGuyManager = ServiceLocator.Current.Get<PromotionGuyManager>();
    }
    
    public override void OnEnter(object args = null)
    {
        Exit exit = args as Exit;
        npc.StopFollowing();
        npc.MoveTo(exit.transform.position - Vector3.down * 3.0f);
        // npc.gameObject.SetActive(false);
        npc.sprite.GetComponent<SpriteRenderer>().DOFade(0.0f, 1.0f).OnKill(() =>
        {
            npc.gameObject.SetActive(false);
        });
        _promotionGuyManager.SendOnePromotionGuyToPrison();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}