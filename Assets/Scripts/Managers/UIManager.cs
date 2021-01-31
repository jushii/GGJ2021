using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameService
{
    public List<Sprite> cutsceneSprites;
    private int _cutSceneSpriteIndex = 0;
    public Image cutsceneImage;
    public GameObject titleScreen;
    
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private TextMeshProUGUI kidsCountLabel;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private Transform kidsPanel;
    [SerializeField] private Transform timerPanel;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private GameObject endScreen;
    
    private Tweener kidCountBounceTweener;
    private EntityManager _entityManager;

    public static bool IsCutsceneActive = true;
    
    private void Awake()
    {
        uiCanvas.enabled = false;
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            int nextCutsceneSpriteIndex = _cutSceneSpriteIndex++;
            if (nextCutsceneSpriteIndex == cutsceneSprites.Count - 1)
            {
                _cutSceneSpriteIndex = nextCutsceneSpriteIndex;
                EndCutscene();
            }
            else
            {
                NextCutsceneImage();
            }
        }
    }

    private void NextCutsceneImage()
    {
        cutsceneImage.sprite = cutsceneSprites[_cutSceneSpriteIndex];
    }

    private void EndCutscene()
    {
        cutsceneImage.gameObject.SetActive(false);
        titleScreen.SetActive(true);
        IsCutsceneActive = false;
    }
    
    public void Setup()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        GameManager.gameTimeChanged += OnGameTimeChanged;
        GameManager.onGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        endScreen.gameObject.SetActive(true);
        scoreLabel.text = $"YOU SAVED {ServiceLocator.Current.Get<EntityManager>().players[0].followers.Count} KIDS! GREAT JOB!";
    }

    private void OnGameTimeChanged(string obj)
    {
        timerLabel.text = obj;
    }

    public void OnStartGame()
    {
        Player player = _entityManager.players[0];

        kidsCountLabel.text = $"x 0";
        player.kidCount.Changed += OnKidCountChanged;

        uiCanvas.enabled = true;

        // timerPanel.transform.DOLocalRotate(new Vector3(0, 0, -2), 1.0f).SetLoops(-1, LoopType.Yoyo);
        // kidsPanel.transform.DOLocalRotate(new Vector3(0, 0, -2), 1.0f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnKidCountChanged(object sender, Observable<int>.ChangedEventArgs e)
    {
        kidsCountLabel.text = $"x {e.NewValue}";

        if (kidCountBounceTweener != null)
        {
            kidCountBounceTweener.Kill();
            kidsCountLabel.transform.localScale = Vector3.one;
        }
        
        kidCountBounceTweener = kidsCountLabel.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 0, 1.0f)
            .OnKill(() =>
            {
                kidsCountLabel.transform.localScale = Vector3.one;
            });
    }
}
