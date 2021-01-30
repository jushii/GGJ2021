using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private EntityManager entityManager;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PromotionGuyManager promotionGuyManager;
    [Header("Prefabs")] 
    [SerializeField] private GameObject player0Prefab;
    [SerializeField] private GameObject player1Prefab;
    
    private void Awake()
    {
        ServiceLocator.Initialize();
        
        // Register all game services.
        ServiceLocator.Current.Register(entityManager);
        ServiceLocator.Current.Register(aiManager);
        ServiceLocator.Current.Register(promotionGuyManager);
        
        // Setup game services.
        aiManager.Setup();
        promotionGuyManager.Setup();
    }
}
