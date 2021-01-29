using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private EntityManager entityManager;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private PlayerInputManager playerInputManager;
    [Header("Prefabs")] 
    [SerializeField] private GameObject player0Prefab;
    [SerializeField] private GameObject player1Prefab;

    private int _playerCount = 0;
    
    private void Awake()
    {
        ServiceLocator.Initialize();
        
        // Register all game services.
        ServiceLocator.Current.Register(entityManager);
        ServiceLocator.Current.Register(aiManager);
        
        // Setup game services.
        aiManager.Setup();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        _playerCount++;
        playerInputManager.playerPrefab = player1Prefab;
    }
    
    private void SetupSystems()
    {
        // InitializeAI();
    }
    
    private void InitializeAI()
    {
        // Player[] players = FindObjectsOfType<Player>();
        //
        // for (int i = 0; i < players.Length; i++)
        // {
        //     Player npc = players[i];
        //     entityManager.RegisterPlayer(npc);
        // }
        //
        // NPC[] npcs = FindObjectsOfType<NPC>();
        //
        // for (int i = 0; i < npcs.Length; i++)
        // {
        //     NPC npc = npcs[i];
        //     entityManager.RegisterNPC(npc);
        // }
    }
}
