using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private EntityManager entityManager;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PromotionGuyManager promotionGuyManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioManager;

    [Header("Other")] 
    [SerializeField] private Camera idleCam;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Canvas titleCanvas;
    
    public bool _isGameStarted;
    
    private void Awake()
    {
        ServiceLocator.Initialize();
        
        // Register all game services.
        ServiceLocator.Current.Register(entityManager);
        ServiceLocator.Current.Register(aiManager);
        ServiceLocator.Current.Register(promotionGuyManager);
        ServiceLocator.Current.Register(uiManager);
        ServiceLocator.Current.Register(audioManager);

        // Setup game services.
        aiManager.Setup();
        promotionGuyManager.Setup();
        uiManager.Setup();
    }

    private void Update()
    {
        if (!_isGameStarted && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        _isGameStarted = true;

        titleCanvas.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f);
        
        Sequence startSequence = DOTween.Sequence();
        startSequence.Insert(0, idleCam.DOOrthoSize(8.0f, 1.0f));
        startSequence.Insert(0, idleCam.transform.DOMoveX(playerSpawn.transform.position.x, 1.0f));
        startSequence.Insert(0, idleCam.transform.DOMoveY(playerSpawn.transform.position.y, 1.0f));
        startSequence.OnKill(() =>
        {
            GameObject player = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
            Player p = player.GetComponent<Player>();
            ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(p);
            
            idleCam.enabled = false;
            uiManager.OnStartGame();
        });
    }
}
