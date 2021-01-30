using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PromotionGuyManager : MonoBehaviour, IGameService
{
    [SerializeField] private Transform prisonTelerportPoint;
    [SerializeField] private List<Transform> teleportPoints;
    
    private AIManager _aiManager;
    private EntityManager _entityManager;
    private List<PromotionGuy> _promotionGuysInPrison = new List<PromotionGuy>();
    private List<PromotionGuy> _promotionGuysInWild = new List<PromotionGuy>();

    public int PromotionGuysInWildCount => _promotionGuysInWild.Count;
    
    public void Setup()
    {
        _aiManager = ServiceLocator.Current.Get<AIManager>();
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        _promotionGuysInPrison = FindObjectsOfType<PromotionGuy>().ToList();
    }

    public void SpawnPromotionGuy()
    {
        // Get random spawn point.
        Transform randomSpawn = teleportPoints[Random.Range(0, teleportPoints.Count)];

        // Get promotion guy from prison.
        PromotionGuy promotionGuy = _promotionGuysInPrison[0];
        _promotionGuysInPrison.Remove(promotionGuy);
        
        // Teleport promotion guy to wild!
        _promotionGuysInWild.Add(promotionGuy);
        
        // Teleport!
        promotionGuy.aiPath.Teleport(randomSpawn.transform.position);
        
        // Start chasing a kid!
        _aiManager.ChangeState(promotionGuy, typeof(PromotionGuy_ChaseKid));
    }

    public void SendPromotionGuyToPrison(PromotionGuy promotionGuy)
    {
        _promotionGuysInWild.Remove(promotionGuy);
        _aiManager.ChangeState(promotionGuy, typeof(PromotionGuy_Idle));
        promotionGuy.aiPath.Teleport(prisonTelerportPoint.position);
        _promotionGuysInPrison.Add(promotionGuy);
    }
    
    public void SendOnePromotionGuyToPrison()
    {
        PromotionGuy promotionGuy = _promotionGuysInWild[0];
        _promotionGuysInWild.Remove(promotionGuy);
        _aiManager.ChangeState(promotionGuy, typeof(PromotionGuy_Idle));
        promotionGuy.aiPath.Teleport(prisonTelerportPoint.position);
        _promotionGuysInPrison.Add(promotionGuy);
    }
    
    public void SendAllPromotionGuysToPrison()
    {
        foreach (PromotionGuy promotionGuy in _promotionGuysInWild)
        {
            _aiManager.ChangeState(promotionGuy, typeof(PromotionGuy_Idle));
            promotionGuy.aiPath.Teleport(prisonTelerportPoint.position);
            _promotionGuysInPrison.Add(promotionGuy);
        }

        _promotionGuysInWild.Clear();
    }
}
