using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    public static Action onPlayerSpawned;
    public static Action<string> gameTimeChanged;
    public static Action onGameOver;
    public static bool isGameOver = false;
    
    public bool _isGameStarted;
    private float _gameTime = 125.0f;
    private float _gameTimer = 0.0f;
    private string formattedTime;

    private bool keyPressedOnce = false;
    
    private void Awake()
    {
        UIManager.IsCutsceneActive = true;
        _isGameStarted = false;
        isGameOver = false;
        _gameTimer = _gameTime;

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
        if (Keyboard.current.escapeKey.IsPressed())
        {
            Application.Quit();
        }
        
        if (Keyboard.current.rKey.IsPressed())
        {
            SceneManager.LoadScene(0);
        }
        
        if (!UIManager.IsCutsceneActive && !_isGameStarted && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (keyPressedOnce)
            {
                StartGame();
            }
            
            keyPressedOnce = true;
        }

        if (_isGameStarted && !isGameOver)
        {
            _gameTimer -= Time.deltaTime;
            string fTime = GetFormattedTime(_gameTimer);
            if (!string.Equals(formattedTime, fTime))
            {
                formattedTime = fTime;
                gameTimeChanged?.Invoke(formattedTime);
            }

            if (_gameTimer <= 0)
            {
                gameTimeChanged?.Invoke("0:00");

                isGameOver = true;
                onGameOver?.Invoke();
            }
        }
    }

    private void StartGame()
    {
        _isGameStarted = true;

        titleCanvas.GetComponent<CanvasGroup>().DOFade(0.0f, 0.5f);
        
        Sequence startSequence = DOTween.Sequence();
        startSequence.Insert(0, idleCam.DOOrthoSize(8.0f, 0.5f));
        startSequence.Insert(0, idleCam.transform.DOMoveX(playerSpawn.transform.position.x, 0.5f));
        startSequence.Insert(0, idleCam.transform.DOMoveY(playerSpawn.transform.position.y, 0.5f));
        startSequence.OnKill(() =>
        {
            GameObject player = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
            Player p = player.GetComponent<Player>();
            ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(p);
            onPlayerSpawned?.Invoke();

            foreach (NPC npc in entityManager.npcs)
            {
                if (npc is Kid) continue;

                entityManager.players[0].onComboEnd += npc.ResetReceivedHits;
            }
            
            idleCam.enabled = false;
            uiManager.OnStartGame();
        });
    }
    
    private string GetFormattedTime(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return $"{minutes:0}:{seconds:00}";
    }
}
