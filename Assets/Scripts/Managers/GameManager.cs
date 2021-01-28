using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EntityManager entityManager;
    [SerializeField] private AIManager aiManager;
    
    private void Awake()
    {
        ServiceLocator.Initialize();
        
        // Register all game services.
        ServiceLocator.Current.Register(entityManager);
        ServiceLocator.Current.Register(aiManager);
        
        // Setup game services.
        aiManager.Setup();
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
