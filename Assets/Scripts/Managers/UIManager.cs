using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameService
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private TextMeshProUGUI kidsCountLabel;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private Transform kidsPanel;
    [SerializeField] private Transform timerPanel;
    
    private Tweener kidCountBounceTweener;
    private EntityManager _entityManager;

    private void Awake()
    {
        uiCanvas.enabled = false;
    }

    public void Setup()
    {
        _entityManager = ServiceLocator.Current.Get<EntityManager>();
        GameManager.gameTimeChanged += OnGameTimeChanged;
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
